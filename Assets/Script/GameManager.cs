using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {
	
	public GameObject CubeDark;
	public GameObject CubeLight;
	public GameObject[] PiecesGO = new GameObject[6];
	public int activePlayer = 1;  // 1 = White, -1 = Dark
	public bool player1AI    = false;  // Bool that state if player1 is a AI
	public bool player2AI    = false;  // Bool that state if player2 is a AI
	public Material DarkMat;
	public Material LightMat;
	public int gameState = 0;			// In this state, the code is waiting for : 0 = Piece selection, 1 = Piece animation
	
	
	private GameObject SelectedPiece;	// Selected Piece
	private int _boardHeight = -1;
	private int _pieceHeight =  0;
	private static int _boardSize   =  8;
	private int[,] _boardPieces = new int[_boardSize,_boardSize];
	void OnGUI()
	{
		string _activePlayerColor;
		if(activePlayer == 1)
			_activePlayerColor = "White";
		else
			_activePlayerColor = "Dark";
		
		
		GUI.Label (new Rect(10,10,200,20), ("Active Player = " + _activePlayerColor));	
		GUI.Label (new Rect(10,30,200,20), ("Game State = " + gameState));
	}
	
	// Initialize the board area
	void Start()
	{
		CreateBoard();
		//AddPieces960();
	}
	
	// Create the board by placing cubes
	void CreateBoard()
	{
		
		
		for(int i = 0; i < _boardSize; i++)
		{
			for(int j = 0; j < _boardSize; j++)
			{
				if((i+j)%2 == 0)
				{
					Object.Instantiate(CubeDark,new Vector3(i,_boardHeight,j), Quaternion.identity);	
				}
				else
				{
					Object.Instantiate(CubeLight,new Vector3(i,_boardHeight,j), Quaternion.identity);
				}
			}
		}
	}
    public void AddPieces960()
    {
        int _linePosY = 0;
        int _piecePlayer = 1;
        int[] spots = new int[7];

        // Create all pawn at once
        for (int i = 0; i < _boardSize; i++)
        {
            CreatePiece("Pawn", i, 1, 1); // Create all white pawn 	
            CreatePiece("Pawn", i, 6, -1); // Create all dark pawn
        }
        //dldldldl y p 0-7
		//place king first
        int K1_spot = Random.Range(1, 7);
        CreatePiece("King", K1_spot, _linePosY, _piecePlayer);
        CreatePiece("King", K1_spot, 7, -1);

        //place rooks 
        int R1_spot = Random.Range(0, K1_spot);
        int R2_spot = Random.Range(K1_spot+1, 8);

        CreatePiece("Rook", R1_spot, _linePosY, _piecePlayer);
        CreatePiece("Rook", R2_spot, _linePosY, _piecePlayer);
        CreatePiece("Rook", R1_spot, 7, -1);
        CreatePiece("Rook", R2_spot, 7, -1);
		spots[0] = K1_spot;
		spots[1] = R1_spot;
		spots[2] = R2_spot;


		bool valid = false;
		int B1_spot = new int();
        //places W Bishop  on a black place
        do
		{ 
			B1_spot = Random.Range(0, 4);//0123
			if (!spots.Contains<int>(value: B1_spot*2))//0246
			{
				valid = true;
			}
		} while (valid == false);

        //light 
        CreatePiece("Bishop", B1_spot * 2, _linePosY, _piecePlayer);
        //dark
        CreatePiece("Bishop", B1_spot * 2, 7, -1);

		spots[3] = B1_spot*2;

        //place second bishop but on oposit color
        int B2_spot = new int();
		valid= false;
        
		do
		{
            B2_spot = Random.Range(0, 8);
			if(!spots.Contains<int>(value: B2_spot)&& B2_spot== 1|| B2_spot == 3 || B2_spot == 5 ||B2_spot ==7)
			{
				valid = true;
			}

		}while(valid == false);
	
        CreatePiece("Bishop", B2_spot , _linePosY, _piecePlayer);
        CreatePiece("Bishop", B2_spot , 7, -1);
        spots[4] = B2_spot;

		valid= false;
        int Kn1_spot=new int();
		int Kn2_spot=new int();
		//find one open spot 
        do
        {
            Kn1_spot = Random.Range(0, 8);
            if (!spots.Contains<int>(value: Kn1_spot) )
            {
                valid = true;
            }
        } while (valid == false);
		valid= false;
		spots[5] = Kn1_spot;
		//find second open spot 
		do
        {
            Kn2_spot = Random.Range(0, 8);
            if (!spots.Contains<int>(value: Kn2_spot) )
            {
                valid = true;
            }
        } while (valid == false);
		valid=	false;
        spots[6] = Kn2_spot;
		//place knights
        CreatePiece("Knight", Kn1_spot, _linePosY, _piecePlayer);
        CreatePiece("Knight", Kn2_spot, _linePosY, _piecePlayer);
        CreatePiece("Knight", Kn1_spot, 7, -1);
        CreatePiece("Knight", Kn2_spot, 7, -1);

		//get the last sopt for the queen but checking the array
		int Q_spot =new int();
        do
        {
            Q_spot = Random.Range(0, 8);
            if (!spots.Contains<int>(value: Q_spot))
            {
                valid = true;
            }
        } while (valid == false);

        CreatePiece("Queen", Q_spot,  _linePosY, _piecePlayer);
        CreatePiece("Queen", Q_spot,  7, -1);

    }

   
    /*//test 
	 * Test_rookking(){
	 * roock1 p <king 
	 * rook2 p >king 
	 * 
	}
	//*/
    // Add all pieces are their respective position

    public void AddPieces()
	{
		int _linePosY;
		int _piecePlayer;
		
		// Create all pawn at once
		for(int i = 0; i < _boardSize; i++)
		{
			CreatePiece("Pawn", i, 1, 1); // Create all white pawn 	
			CreatePiece("Pawn", i, 6, -1); // Create all dark pawn
		}
		
		// Create white pieces
		_linePosY = 0;
		_piecePlayer = 1;
		CreatePiece("Rook"  , 0, _linePosY, _piecePlayer);
		CreatePiece("Knight", 1, _linePosY, _piecePlayer);
		CreatePiece("Bishop", 2, _linePosY, _piecePlayer);
		CreatePiece("Queen" , 3, _posY: _linePosY, _piecePlayer);
		CreatePiece("King"  , 4, _linePosY, _piecePlayer);
		CreatePiece("Bishop", 5, _linePosY, _piecePlayer);
		CreatePiece("Knight", 6, _linePosY, _piecePlayer);
		CreatePiece("Rook"  , 7, _linePosY, _piecePlayer);
		
		// Create Dark pieces
		_linePosY = 7;
		_piecePlayer = -1;
		CreatePiece("Rook"  , 0, _linePosY, _piecePlayer);
		CreatePiece("Knight", 1, _linePosY, _piecePlayer);
		CreatePiece("Bishop", 2, _linePosY, _piecePlayer);
		CreatePiece("Queen" , 3, _linePosY, _piecePlayer);
		CreatePiece("King"  , 4, _linePosY, _piecePlayer);
		CreatePiece("Bishop", 5, _linePosY, _piecePlayer);
		CreatePiece("Knight", 6, _linePosY, _piecePlayer);
		CreatePiece("Rook"  , 7, _linePosY, _piecePlayer);
	}
	
	// Spawn a piece on the board
	void CreatePiece(string _pieceName, int _posX, int _posY, int _playerTag)
	{
		GameObject _PieceToCreate = null;
		int 	   _pieceIndex = 0;
		//Select the right prefab to instantiate
		switch (_pieceName)
		{
		    case "Pawn": 
				_pieceIndex = 1;
		        break;
			case "Rook": 
				_pieceIndex = 2;
		        break;
			case "Knight": 
				_pieceIndex = 3;
		        break;
			case "Bishop": 
				_pieceIndex = 4;
		        break;
			case "Queen": 
				_pieceIndex = 5;
		        break;
			case "King": 
				_pieceIndex = 6;
		        break;
		}
		_PieceToCreate = PiecesGO[_pieceIndex-1];
		// Instantiate the piece as a GameObject to be able to modify it after
		_PieceToCreate = Object.Instantiate (_PieceToCreate,new Vector3(_posX, _pieceHeight, _posY), Quaternion.identity) as GameObject;
		_PieceToCreate.name = _pieceName;
		
		//Add material to the piece and tag it
		if(_playerTag == 1)
		{
			_PieceToCreate.tag = "1";
			_PieceToCreate.GetComponent<Renderer>().material = LightMat;
		}
		else if(_playerTag == -1)
		{
			_PieceToCreate.tag = "-1";
			_PieceToCreate.GetComponent<Renderer>().material = DarkMat;		
		}
		
		_boardPieces[_posX,_posY] = _pieceIndex*_playerTag; 
	}
	
	//Update SlectedPiece with the GameObject inputted to this function
	public void SelectPiece(GameObject _PieceToSelect)
	{
		// Unselect the piece if it was already selected
		if(_PieceToSelect  == SelectedPiece)
		{
			SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
			SelectedPiece = null;
			ChangeState (0);
		}
		else
		{
			// Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
			if(SelectedPiece)
			{
				SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
			}
			SelectedPiece = _PieceToSelect;
			SelectedPiece.GetComponent<Renderer>().material.color = Color.blue;
			ChangeState (1);
		}
	}
	
	// Move the SelectedPiece to the inputted coords
	public void MovePiece(Vector2 _coordToMove)
	{
		bool validMovementBool = false;
		Vector2 _coordPiece = new Vector2(SelectedPiece.transform.position.x, SelectedPiece.transform.position.z);
		
		// Don't move if the user clicked on its own cube or if there is a piece on the cube
		if((_coordToMove.x != _coordPiece.x || _coordToMove.y != _coordPiece.y) || _boardPieces[(int)_coordToMove.x,(int)_coordToMove.y] != 0)
		{
			validMovementBool	= TestMovement (SelectedPiece, _coordToMove);
		}
		
		if(validMovementBool)
		{
			_boardPieces[(int)_coordToMove.x, (int)_coordToMove.y] = _boardPieces[(int)_coordPiece.x, (int)_coordPiece.y];
			_boardPieces[(int)_coordPiece.x , (int)_coordPiece.y ] = 0;
			
			SelectedPiece.transform.position = new Vector3(_coordToMove.x, _pieceHeight, _coordToMove.y);		// Move the piece
			SelectedPiece.GetComponent<Renderer>().material.color = Color.white;	// Change it's color back
			SelectedPiece = null;									// Unselect the Piece
			ChangeState (0);
			activePlayer = -activePlayer;
		}
	}
	
	// If the movement is legal, eat the piece
	public void EatPiece(GameObject _PieceToEat)
	{
		Vector2 _coordToEat = new Vector2(_PieceToEat.transform.position.x, _PieceToEat.transform.position.z);
		int _deltaX = (int)(_PieceToEat.transform.position.x - SelectedPiece.transform.position.x);
		
		if(TestMovement (SelectedPiece, _coordToEat) && (SelectedPiece.name != "Pawn" ||  _deltaX != 0))
		{
			Object.Destroy (_PieceToEat);
			_boardPieces[(int)_coordToEat.x, (int)_coordToEat.y] = 0; 
			MovePiece(_coordToEat);
		}
	}
	
	// Test if the piece can do the player's movement
	bool TestMovement(GameObject _SelectedPiece, Vector2 _coordToMove)
	{
		bool _movementLegalBool = false;
		bool _collisionDetectBool = false;
		Vector2 _coordPiece = new Vector2(_SelectedPiece.transform.localPosition.x, _SelectedPiece.transform.localPosition.z);
		
		int _deltaX = (int)(_coordToMove.x - _coordPiece.x);
		int _deltaY = (int)(_coordToMove.y - _coordPiece.y);
		int activePlayerPawnPostion = 1;
		//Debug.Log("Piece (" + _coordPiece.x + "," + _coordPiece.x + ") - Move (" + _coordToMove.x + "," + _coordToMove.y + ")");
		//Debug.Log("Delta (" + _deltaX + "," + _deltaY + ")");
		// Use the name of the _SelectedPiece GameObject to find the piece used
		switch (_SelectedPiece.name)
		{
			
		    case "Pawn": 
				if(activePlayer == -1)
					activePlayerPawnPostion = 6;		
			
			
				// Pawn can move 1 steap ahead of them, 2 if they are on their initial position
		        if(_deltaY == activePlayer || (_deltaY == 2*activePlayer && _coordPiece.y == activePlayerPawnPostion))
				{
				//Debug.Log ("_deltaY");
					_movementLegalBool = true;
				}
				
		        break;
		    case "Rook":
				// Rook can move horizontally or vertically
				if((_deltaX != 0 && _deltaY == 0) || (_deltaX == 0 && _deltaY != 0)) 
				{
					_movementLegalBool = true;
				}
		        break;
			case "Knight":
				// Knight move in a L movement. distance is evaluated by a multiplication of both direction
				if((_deltaX != 0 && _deltaY != 0) && Mathf.Abs(_deltaX*_deltaY) == 2)
				{
					_movementLegalBool = true;
				}
		        break;
			case "Bishop":
				// Bishop can only move diagonally
				if(Mathf.Abs(_deltaX/_deltaY) == 1)
				{
					_movementLegalBool = true;
				}
		        break;
			
			case "Queen":
				// Queen movement is a combination of Rook and bishop
				if(((_deltaX != 0 && _deltaY == 0) || (_deltaX == 0 && _deltaY != 0)) || Mathf.Abs(_deltaX/_deltaY) == 1)
				{
					_movementLegalBool = true;
				}
		        break;
			
			case "King":
				// Bishop can only move diagonally
				if(Mathf.Abs(_deltaX + _deltaY) == 1) 
				{
					_movementLegalBool = true;
				}
		        break;
			
		    default:
		        _movementLegalBool = false;
		        break;
		}
		
		// If the movement is legal, detect collision with piece in the way. Don't do it with knight since they can pass over pieces.
		if(_movementLegalBool && SelectedPiece.name != "Knight")
		{
			_collisionDetectBool = TestCollision (_coordPiece, _coordToMove);
		}
			
		return (_movementLegalBool && !_collisionDetectBool);
	}
	
	// Test if a unit is in the path of the tested movement
	bool TestCollision(Vector2 _coordInitial,Vector2 _coordFinal)
	{
		bool CollisionBool = false;
		int _deltaX = (int)(_coordFinal.x - _coordInitial.x);
		int _deltaY = (int)(_coordFinal.y - _coordInitial.y);
		int _incX = 0; // Direction of the incrementation in X
		int _incY = 0; // Direction of the incrementation in Y
		int i;
		int j;
		
		// Calculate the increment if _deltaX/Y is different from 0 to avoid division by 0
		if(_deltaX != 0)
		{
			_incX = (_deltaX/Mathf.Abs(_deltaX));
		}
		if(_deltaY != 0)
		{
			_incY = (_deltaY/Mathf.Abs(_deltaY));
		}
		
		i = (int)_coordInitial.x + _incX;
		j = (int)_coordInitial.y + _incY;
		
		while(new Vector2(i, j) != _coordFinal)
		{
					
			if(_boardPieces[i,j] != 0)
			{
				CollisionBool = true;
				break;
			}
			
			i += _incX;
			j += _incY;
		}
		Debug.Log (CollisionBool);
		return CollisionBool;
	}

	// Change the state of the game
	public void ChangeState(int _newState)
	{
		gameState = _newState;
	}
}
