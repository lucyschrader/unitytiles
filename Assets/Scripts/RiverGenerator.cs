using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverGenerator : MonoBehaviour {

	public int width;
	public int height;

	public int currentTiles = 0;
	public int totalTiles;

	public Transform tile;
	public Transform sphere;

	// Tile textures
	public Texture2D terminalTexture;
	public Texture2D straightTexture;
	public Texture2D bendTexture;
	public Texture2D teeTexture;
	public Texture2D crossTexture;

	int [,] map;

	// What the helllllll
	public class PlayedTile
	{
		public Transform tile;
		public Texture2D tileTexture;

		public bool isFirst;

		public List<> nodePoints;
		public string tileOrientation;

		public PlayedTile (Transform tile)
		{
			tile = tile;
		}

		public void AddTexture() {
			
		}

		public void firstTile() {
		
		}

	}

	public void Start () {
		GenerateMap ();
	}

	public void Update () {
		if (Input.GetMouseButtonDown (0)) {
			currentTiles = 0;
			GenerateMap ();
		}
	}

	// Clears the board and fires off the tile placement functions
	public void GenerateMap() {
		// Declare the first tile to be the first tile, so the rest know they aren't


		GameObject[] oldTiles = GameObject.FindGameObjectsWithTag("Tile");
		foreach (GameObject tile in oldTiles) {
			Destroy(tile);
		}
		GameObject[] oldSpheres = GameObject.FindGameObjectsWithTag ("GridMarker");
		foreach (GameObject sphere in oldSpheres) {
			Destroy (sphere);
		}
		map = new int[width, height];
		PlaceMarkers ();
		PlaceFirstTile();
	}

	// Show me where the board at!
	public void PlaceMarkers() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Instantiate (sphere, new Vector3 (x, y, 0), Quaternion.identity);
			}
		}
	}

	public void PlaceFirstTile() {

		int tileX = Random.Range (0, width-1);
		int tileY = Random.Range (0, height-1);

		int[] rotationOptions = FindRotations(tileX,tileY);

		int randomSelection = Random.Range (0, rotationOptions.Length);
		int tileRotation = rotationOptions[randomSelection];

		Transform firstTile = Instantiate (tile, new Vector3 (tileX, tileY, 0), Quaternion.Euler(new Vector3(0,0,tileRotation)));
		map [tileX, tileY] = 1;

		string textureID = AddTexture(firstTile, true);

		List<int> nodePlacement = GetNodePlacement (textureID, tileRotation);

		currentTiles += 1;

		// Run through the nodes coming off the current tile, check possible options
//		for (int in nodePlacement) {
		// if i would hit an existing tile
		// if i would go off board
//		}

		// CheckNextTile (tileX, tileY, nodePlacement;

		PlaceNextTile (tileX, tileY, nodePlacement);

	}

	public void CheckNextTile(int tileX, int tileY, int nodePlacement) {
		int newTileX;
		int newTileY;

		string newTileTexture;

		List<int> usableTextures = new List<int>("terminal", "straight", "bend");
		usableTextures.Remove ("terminal");
	// Cannot let the path hit an existing tile or the edge of the board
	// Check which tiles are an option, remove if needed
		if (nodePlacement == 1) {
			newTileX = tileX;
			newTileY = tileY + 1;

			if (newTileY == height) {
				usableTextures.Remove ("straight");
			}
		}

		if (nodePlacement == 2) {
			newTileX = tileX + 1;
			newTileY = tileY;

			if (newTileX == width) {
				usableTextures.Remove ("straight");
			}
		}

		if (nodePlacement == 3) {
			newTileX = tileX;
			newTileY = tileY - 1;

			if (newTileY == 0) {
				usableTextures.Remove ("straight");
			}
		}

		if (nodePlacement == 4) {
			newTileX = tileX - 1;
			newTileY = tileY;

			if (newTileX == 0) {
				usableTextures.Remove ("straight");
			}
		} 

	// Check which rotations are an option, remove if needed

	// Pick one
		if (usableTextures.Count == 0 {
			newTileTexture = "terminal";
		} else {
			newTileTexture = usableTextures(Random.Range(0,usableTextures.Count));
		}

	// End the path if the next tile would collide
	}

	public void PlaceNextTile(int tileX, int tileY, List<> nodePlacement) {

		Transform newTile = Instantiate (tile, new Vector3 (tileX, tileY, 0), Quaternion.identity);
		int textureID = AddTexture (newTile, false);

		// This is based on straight tiles, needs to use textureID to account for other tiles
		// Also needs to iterate through the nodePlacement list
		if (nodePlacement == 1) {
			newTile.position = new Vector3 (tileX, tileY + 1, 0);
			newTile.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
			tileY = tileY + 1;
		}

		if (nodePlacement == 2) {
			newTile.position = new Vector3 (tileX + 1, tileY, 0);
			newTile.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			tileX = tileX + 1;
		}

		if (nodePlacement == 3) {
			newTile.position = new Vector3 (tileX, tileY - 1, 0);
			newTile.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
			tileY = tileY - 1;
		}

		if (nodePlacement == 4) {
			newTile.position = new Vector3 (tileX - 1, tileY, 0);
			newTile.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			tileX = tileX - 1;
		}

		map[tileX, tileY] = 1;
		currentTiles += 1;

//		int[] rotationOptions = FindRotations (tileX, tileY);
//		int nodePlacement = GetNodePlacement (textureID, tileRotation);

		if (currentTiles < totalTiles) {
			PlaceNextTile (tileX, tileY, nodePlacement);
		}
	}

	// Remove unacceptable rotations
	public int[] FindRotations(int x,int y) {
		int[] rotationOptions = {0,90,180,270};
		List<int> numbersToPop = new List<int>();

		if (x == 0) {
			numbersToPop.Add (0);
		}
		if (x == width-1) {
			numbersToPop.Add (180);
		}
		if (y == 0) {
			numbersToPop.Add (90);
		}
		if (y == height-1) {
			numbersToPop.Add (270);
		}

		// Pop unwanted numbers out of rotationOptions
		foreach (int num in numbersToPop) {
			int[] newRotOptions = new int[rotationOptions.Length - 1];

			int i = 0;
			int j = 0;
			while (i < rotationOptions.Length) {
				if (rotationOptions[i] != num) {
					newRotOptions[j] = rotationOptions[i];
					j++;
				}
				i++;
			}
			rotationOptions = newRotOptions;
		}
		return rotationOptions;
	}

	public List<int> GetNodePlacement(string textureID, int tileRotation) {
		List<int> nodePlacement = new List<int>();

		if (textureID == "terminal") {
			if (tileRotation == 0) {
				nodePlacement.Add(4);
			} else if (tileRotation == 90) {
				nodePlacement.Add(3);
			} else if (tileRotation == 180) {
				nodePlacement.Add(2);
			} else if (tileRotation == 270) {
				nodePlacement.Add(1);
			}
		} else if (textureID == "straight") {
			if (tileRotation == 0 || tileRotation == 180) {
				nodePlacement.Add(4);
				nodePlacement.Add(2);
			} else if (tileRotation == 90 || tileRotation == 270) {
				nodePlacement.Add(3);
				nodePlacement.Add(1);
			}
		} else if (textureID == "bend") {
			if (tileRotation == 0) {
				nodePlacement.Add(4);
				nodePlacement.Add (1);
			} else if (tileRotation == 90) {
				nodePlacement.Add(3);
				nodePlacement.Add (4);
			} else if (tileRotation == 180) {
				nodePlacement.Add(2);
				nodePlacement.Add (3);
			} else if (tileRotation == 270) {
				nodePlacement.Add(1);
				nodePlacement.Add (2);
			}
		}
	
		return nodePlacement;
	}
		
	public string AddTexture(Transform tile, bool isFirst) {
		string textureID;

		if (isFirst) {
			tile.GetComponent<Renderer> ().material.mainTexture = terminalTexture;
			textureID = "terminal";
		} else if (Random.Range (0,1) == 0) {
			tile.GetComponent<Renderer> ().material.mainTexture = straightTexture;
			textureID = "straight";
		} else {
			tile.GetComponent<Renderer> ().material.mainTexture = bendTexture;
			textureID = "bend";
		}

		return textureID;
	}
		
}
