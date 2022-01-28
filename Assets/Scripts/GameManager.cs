using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.Collections;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private Material memerial;

    [SerializeField] private int serializeWidth;
    [SerializeField] private int serializeHeight;
    [SerializeField] private string seed;

    private bool loadedSave;
    private GridGenerator gridGen;
    public TileController tileController; // SKA VARA PRIVATE ÄR PUBLIC FÖR ATT TESTA I PLAYER
    private UnitController unitController;
    private ClientRenderer renderer;
    private Renderer testRender;

    public static GameManager main;
    private void Awake()
    {
        main = this;
    }
    public override void OnStartServer()
    {
        //GET SEED
        gridGen = new GridGenerator();

        //Generate World FROM SEED

        if (loadedSave)
        {
            World.CreateWorld(serializeWidth, serializeHeight, gridGen.GenerateWorld(serializeWidth, serializeHeight)); // FUCKING STARTA ALLT EFTER VÄRLDEN ÄR LOADAD
        }
        else
            World.CreateWorld(serializeWidth, serializeHeight, gridGen.GenerateWorld(serializeWidth, serializeHeight));

        tileController = new TileController();
        unitController = new UnitController();

        UnitController.activeUnits.Add(new BaseUnit(new Vector2(50, 50), new Vector2(1, 1), 0.7f, .1f, 1f));

        renderer = new ClientRenderer(material, memerial);
        //testRender = GetComponent<Renderer>();
        //testRender.CreateMesh();
        // renderer.TestingInit();
    }
    private void Update()
    {

        renderer.UpdateBuffer();
        //renderer.RenderScreen();
        //testRender.DrawScreen();
        tileController.CheckActiveTiles();
        if (Input.GetMouseButton(0))
        {
            tileController.MineBlock(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.5f);
        }
        else if (Input.GetMouseButton(1))
        {
            tileController.CreateBlock(Camera.main.ScreenToWorldPoint(Input.mousePosition), TileType.Dirt);
        }

    }
    private void FixedUpdate()
    {
        unitController.HandleUnitMovement();
        tileController.UpdateCells();
    }
}
