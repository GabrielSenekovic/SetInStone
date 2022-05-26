using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Map : MonoBehaviour
{
    public class RoomData
    {
        public List<TileData> tiles;

        public bool discovered;

        public Vector2Int doorPosition; //It's only used to draw the background, nothing else. Has offset applied already

        public string name;

        public RoomData(List<TileData> tiles_in, bool discovered_in, string name_in)
        {
            tiles = tiles_in;
            discovered = discovered_in;
            name = name_in;
        }
    }
    public class TileData
    {
        public Vector2Int position;
        public Vector2Int dimensions;
        public Color color;

        public TileData(Vector2Int position_in, Color32 color_in)
        {
            position = position_in;
            color = (Color)color_in;
            dimensions = new Vector2Int(1,1);
        }
    }
    public Image mapImage;
    Image playerImage;
    Vector2 movement;

    CanvasGroup canvas;

    public float panSpeed;
    public float zoomSpeed;

    public Vector2 playerStartPosition;
    Vector2 pan;

    Vector2 confines; //Only needs two values, as the opposing values mirror eachother

    Texture2D texture;

    [SerializeField]bool debugBackground;

    private void Awake()
    {
        List<Image> temp = GetComponentsInChildren<Image>().ToList();
        mapImage = temp[0];
        playerImage = temp[1];
        movement = Vector2.zero;
        canvas = GetComponent<CanvasGroup>();
        pan = Vector2.zero;

        //40 and 460 are the confines for 2000
        UpdateConfines();
    }

    public void Open()
    {
        //Times 3, because the map defaults to 1000 * 3
        //+1080, because the coordinate system of the texture is in the top left, and the coordinate system of the canvas is in the bottom left

        UpdateTokenPositions();

        canvas.alpha = 1;
    }
    public void Close()
    {
        canvas.alpha = 0;
    }
    private void FixedUpdate()
    {
        UpdatePosition();
    }
    public void CreateMapTexture(List<RoomData> rooms)
    {
        texture = new Texture2D(1000, 1000, TextureFormat.ARGB32, false);

        FillMapBackground();

        // set the pixel values
        for(int i = 0; i < rooms.Count; i++)
        {
            if(!rooms[i].discovered){continue;}
            for(int j = 0; j < rooms[i].tiles.Count; j++)
            {
                texture.SetPixel((int)rooms[i].tiles[j].position.x, (int)rooms[i].tiles[j].position.y, rooms[i].tiles[j].color);
            }
            FillRoomBackground(rooms[i]);
        }

        // Apply all SetPixel calls
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        
        var temp = Sprite.Create(texture, new Rect(.0f,.0f, texture.width, texture.height), new Vector2(.5f,.5f), 16);
        mapImage.sprite = temp;
    }
    void FillMapBackground()
    {
        //Fills the background of the entire level
        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 1000; j++)
            {
                texture.SetPixel(i, j, (Color)new Color32(50, 50, 50, 255));
            }
        }
    }

    void FillRoomBackground(RoomData room)
    {
        //Fills the background of each room
        //Figure out the starting position for the function
        List<Vector2Int> listOfBackgroundTiles = new List<Vector2Int>();
        //int depth = 0;
        try
        {
            OnFillRoomBackground(room.doorPosition, room.tiles, ref listOfBackgroundTiles);
        }
        catch
        {
            Debug.Log("oopsie");
            //Debug.Log("This room gave a Stack Overflow at: " + depth + " amount of recursive functions");
        }

        for(int i = 0; i < listOfBackgroundTiles.Count; i++)
        {
            texture.SetPixel((int)listOfBackgroundTiles[i].x, (int)listOfBackgroundTiles[i].y, Color.yellow * Color.gray);
        }
    }
    void OnFillRoomBackground(Vector2Int position, List<TileData> tiles, ref List<Vector2Int> backgroundTiles)
    {
        Vector2Int currentPosition = new Vector2Int((int)position.x, (int)position.y);
        List<Vector2Int> positionsUp = new List<Vector2Int>(); //List of unchecked tiles up
        List<Vector2Int> positionsDown = new List<Vector2Int>(); //List of unchecked tiles down
        int direction = 0;

        //First, check what direction the door is in
        if(tiles.Any(t => t.position == new Vector2Int(currentPosition.x -1, currentPosition.y)))
        {
            direction = 1;
        }
        if(tiles.Any(t => t.position == new Vector2Int(currentPosition.x +1, currentPosition.y)))
        {
            direction = -1;
        }

        bool saveUp = true; //True if it should save an empty spot. False if it should wait until down or up is a wall
        bool saveDown = true; //Then it is set to true again

        Vector2Int newPosition = new Vector2Int(currentPosition.x + direction, currentPosition.y);
        backgroundTiles.Add(currentPosition);
        do
        {
            //First, check up and down for empty space
            if(!tiles.Any(t => t.position == new Vector2Int(currentPosition.x, currentPosition.y+1))) //Check if up is a wall
            {
                if(saveUp)
                {
                    positionsUp.Add(new Vector2Int(currentPosition.x, currentPosition.y+1)); //Add it to be checked later
                    saveUp = false; //Wait until you find a wall block up until you can save again
                }
            }
            else
            {
                saveUp = true; //You found a wall block, so you can save up positions again
            }
            if(!tiles.Any(t => t.position == new Vector2Int(currentPosition.x, currentPosition.y-1))) //Check if down is a wall
            {
                if(saveDown)
                {
                    positionsDown.Add(new Vector2Int(currentPosition.x, currentPosition.y-1)); //Add it to be checked later
                    saveDown = false; //Wait until you find a wall block down until you can save again
                }
            }
            else
            {
                saveDown = true; //You found a wall block, so you can save down positions again
            }
            backgroundTiles.Add(newPosition); //Add it to the list of background tiles
            currentPosition = newPosition; //Reset currentPosition to the one you just checked
            newPosition = new Vector2Int(currentPosition.x + direction, currentPosition.y); //Recalculate new position
        }
        while(!tiles.Any(t => t.position == newPosition)); //If the next position is not a wall, continue the loop
        //If it was not a wall, this for loop will now end and the while loop will run

        //Now, go up and down, prioritizing up, in the proper direction prioritizing the first direction (right/left), until you run out of positions to check
        OnFillRoomBackground_CheckUpAndDown(positionsUp, positionsDown, direction, tiles, ref backgroundTiles);
    }

    void OnFillRoomBackground_CheckUpAndDown(List<Vector2Int> positionsUp, List<Vector2Int> positionsDown, int direction, List<TileData> tiles, ref List<Vector2Int> backgroundTiles)
    {
        bool saveUp = true; //True if it should save an empty spot. False if it should wait until down or up is a wall
        bool saveDown = true; //Then it is set to true again

        bool cont = true;

        while(positionsUp.Count > 0 || positionsDown.Count > 0) //Loop until both lists are empty
        {
            if(!cont){return;}
            cont = OnFillRoomBackground_CheckUpOrDown(ref positionsUp  , ref positionsDown, tiles, direction,  1, saveUp  , saveDown  , ref backgroundTiles);
            if(!cont){return;}
            cont = OnFillRoomBackground_CheckUpOrDown(ref positionsDown, ref positionsUp  , tiles, direction, -1, saveDown, saveUp    , ref backgroundTiles);
        }
    }
    bool OnFillRoomBackground_CheckUpOrDown(ref List<Vector2Int> positions, ref List<Vector2Int> otherPositions, List<TileData> tiles, int directionHorizontal, int directionVertical, bool mainSaveDirection, bool secSaveDirection, ref List<Vector2Int> backgroundTiles)
    {
        Vector2Int currentPosition = Vector2Int.zero;
        bool savedPosition = false; //On the first tile of each new line, check if theres more in the other direction before proceeding

        Vector2Int lastLinesEndPosition = Vector2Int.zero; //The position where it ended last line in the primary direction
        Vector2Int lastLinesEndPositionSec = Vector2Int.zero; //The position where it ended last line in the other direction

        List<Vector2Int> positions_copy = positions;
        List<Vector2Int> backgroundTiles_copy = backgroundTiles;

        for(int i = 0; i < positions.Count; i++)
        {
            mainSaveDirection = true;
            secSaveDirection = true;
            //Check tiles down and to the right/left
            //On the absolute first tile, check the other direction, just in case, ie savedPosition = true
            currentPosition = positions[i];

            Vector2Int newPosition = new Vector2Int(currentPosition.x + directionHorizontal, currentPosition.y);
            backgroundTiles.Add(currentPosition); backgroundTiles_copy.Add(currentPosition);

            if(!tiles.Any(t => t.position == new Vector2Int(currentPosition.x + directionHorizontal * -1, currentPosition.y)) && //Check if other direction is a wall
               !backgroundTiles_copy.Any(p => p == new Vector2Int(currentPosition.x + directionHorizontal * -1, currentPosition.y))) //Make sure that it is not already
            {
                savedPosition = true; //Go back to the opposite side of the first position
            }

            do //Now go through every tile in the direction
            {
                if(i > 0 && ((directionHorizontal == -1 && currentPosition.x < lastLinesEndPosition.x) || (directionHorizontal == 1 && currentPosition.x > lastLinesEndPosition.x)))
                {
                    //If going to the left, and the current position is further to the left than last line, then start checking tiles in the opposite direction vertically
                    //Or if going to the right, and the current position is further to the right than last time, then start checking tiles in the opposite direction vertically
                    if(!tiles.Any(t => t.position == new Vector2Int(currentPosition.x, currentPosition.y + directionVertical * -1))) //Check if down is a wall
                    {
                        if(secSaveDirection)
                        {
                            otherPositions.Add(new Vector2Int(currentPosition.x, currentPosition.y + directionVertical * -1)); //Add it to be checked later
                            secSaveDirection = false; //Wait until you find a wall block down until you can save again
                        }
                    }
                    else
                    {
                        secSaveDirection = true; //You found a wall block, so you can save down positions again
                    }
                }
                if(!tiles.Any(t => t.position == new Vector2Int(currentPosition.x, currentPosition.y + directionVertical))) //Check if down is a wall
                {
                    if(mainSaveDirection)
                    {
                        positions.Add(new Vector2Int(currentPosition.x, currentPosition.y + directionVertical)); //Add it to be checked later
                        positions_copy = positions;
                        mainSaveDirection = false; //Wait until you find a wall block down until you can save again
                    }
                }
                else
                {
                    mainSaveDirection = true; //You found a wall block, so you can save down positions again
                }
                backgroundTiles.Add(newPosition); backgroundTiles_copy.Add(newPosition);//Add it to the list of background tiles
                currentPosition = newPosition;
                newPosition = new Vector2Int(currentPosition.x + directionHorizontal, currentPosition.y); //Recalculate new position
            }
            while(!tiles.Any(t => t.position == newPosition && !positions_copy.Any(p => p == newPosition)));
            lastLinesEndPosition = newPosition;

            if(savedPosition)
            {
                mainSaveDirection = true;
                secSaveDirection = true;

                currentPosition = positions[i];
                //Go back to the beginning of the line and go in the opposite direction
                newPosition = new Vector2Int(currentPosition.x + directionHorizontal * -1, currentPosition.y);

                do //Now go through every tile in the direction
                {
                    if( i > 0 && ((directionHorizontal == -1 && currentPosition.x > lastLinesEndPositionSec.x) || (directionHorizontal == 1 && currentPosition.x < lastLinesEndPositionSec.x)))
                    {
                        //If going to the left, and the current position is further to the left than last line, then start checking tiles in the opposite direction vertically
                        //Or if going to the right, and the current position is further to the right than last time, then start checking tiles in the opposite direction vertically
                        if(!tiles.Any(t => t.position == new Vector2Int(currentPosition.x, currentPosition.y + directionVertical * -1))) //Check if down is a wall
                        {
                            if(secSaveDirection)
                            {
                                otherPositions.Add(new Vector2Int(currentPosition.x, currentPosition.y + directionVertical * -1)); //Add it to be checked later
                                secSaveDirection = false; //Wait until you find a wall block down until you can save again
                            }
                        }
                        else
                        {
                            secSaveDirection = true; //You found a wall block, so you can save down positions again
                        }
                    }
                    if(!tiles.Any(t => t.position == new Vector2Int(currentPosition.x, currentPosition.y + directionVertical))) //Check if down/up is a wall
                    {
                        if(mainSaveDirection)
                        {
                            positions.Add(new Vector2Int(currentPosition.x, currentPosition.y + directionVertical)); //Add it to be checked later
                            positions_copy = positions;
                            mainSaveDirection = false; //Wait until you find a wall block down until you can save again
                        }
                    }
                    else
                    {
                        mainSaveDirection = true; //You found a wall block, so you can save down positions again
                    }
                    backgroundTiles.Add(newPosition); backgroundTiles_copy.Add(newPosition); //Add it to the list of background tiles
                    currentPosition = newPosition;
                    newPosition = new Vector2Int(currentPosition.x + directionHorizontal * -1, currentPosition.y); //Recalculate new position
                }
                while(!tiles.Any(t => t.position == newPosition) && !positions_copy.Any(p => p == newPosition));
                //Sometimes, a square added by looking down or up might have been added by another line, so terminate when this occurs
                lastLinesEndPositionSec = newPosition;
                savedPosition = false;
            }
        }
        
        positions.Clear();
        return true;
    }

    public void Zoom(Vector2 value)
    {
        mapImage.rectTransform.sizeDelta = new Vector2(mapImage.rectTransform.rect.width + value.y * zoomSpeed, mapImage.rectTransform.rect.height + value.y * zoomSpeed);
        if(mapImage.rectTransform.sizeDelta.x < 1920)
        {
            mapImage.rectTransform.sizeDelta = new Vector2(1920, 1920);
            UpdateConfines();
            UpdatePosition();
            return;
        }
        if(mapImage.rectTransform.sizeDelta.x > 10000)
        {
            mapImage.rectTransform.sizeDelta = new Vector2(10000, 10000);
            UpdateConfines();
            UpdatePosition();
            return;
        }
        UpdateConfines();
        UpdatePosition();

        //125 is 3000 / 24
        playerImage.rectTransform.sizeDelta = new Vector2(playerImage.rectTransform.rect.width + value.y * zoomSpeed / 125, playerImage.rectTransform.rect.height + value.y * zoomSpeed / 125);
        UpdateTokenPositions();
    }
    public void UpdatePosition()
    {
        mapImage.rectTransform.transform.position = new Vector2(mapImage.rectTransform.transform.position.x - movement.x * panSpeed,
                                                                mapImage.rectTransform.transform.position.y - movement.y * panSpeed);                          

        if(mapImage.rectTransform.transform.localPosition.x > confines.x) {mapImage.rectTransform.transform.localPosition = new Vector2( confines.x, mapImage.rectTransform.transform.localPosition.y); return;}
        if(mapImage.rectTransform.transform.localPosition.x < -confines.x){mapImage.rectTransform.transform.localPosition = new Vector2(-confines.x, mapImage.rectTransform.transform.localPosition.y); return;}
        if(mapImage.rectTransform.transform.localPosition.y > confines.y) {mapImage.rectTransform.transform.localPosition = new Vector2(mapImage.rectTransform.transform.localPosition.x, confines.y); return;}
        if(mapImage.rectTransform.transform.localPosition.y < -confines.y){mapImage.rectTransform.transform.localPosition = new Vector2(mapImage.rectTransform.transform.localPosition.x, -confines.y); return;}

        pan = new Vector2(pan.x - movement.x * panSpeed,
                         pan.y - movement.y * panSpeed); 
        UpdateTokenPositions(); 
    }
    public void UpdateTokenPositions()
    {
        float zoom = (mapImage.rectTransform.sizeDelta.x/1000);

        playerImage.rectTransform.transform.localPosition = new Vector2((Game.GetPlayerPosition().x + playerStartPosition.x) * zoom + pan.x, 
                                                                   (Game.GetPlayerPosition().y + playerStartPosition.y) * zoom + pan.y);
    }
    public void UpdateConfines()
    {
        confines.x = 40 + ((mapImage.rectTransform.sizeDelta.x - 2000)/2);
        confines.y = 460 + ((mapImage.rectTransform.sizeDelta.y - 2000)/2);
    }
    public void SetMovement(Vector2 movement_in)
    {
        movement = movement_in;
    }
}
