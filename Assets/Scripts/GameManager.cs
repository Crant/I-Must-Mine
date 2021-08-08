using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.Collections;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private Material material;

    [SerializeField] private int serializeWidth;
    [SerializeField] private int serializeHeight;
    [SerializeField] private string seed;

    private bool loadedSave;
    private GridGenerator gridGen;
    private TileController tileController;
    private UnitController unitController;
    private ClientRenderer renderer;
    public override void OnStartServer()
    {
        //GET SEED
        gridGen = new GridGenerator();

        //Generate World FROM SEED

        if (loadedSave)
        {
            World.CreateWorld(serializeWidth, serializeHeight, gridGen.GenerateWorld(serializeWidth, serializeHeight)); // FUCKING STARTA ALLT EFTER VÄRLDEN ÄR LOADAD RETARD
        }
        else
            World.CreateWorld(serializeWidth, serializeHeight, gridGen.GenerateWorld(serializeWidth, serializeHeight));

        tileController = new TileController();
        unitController = new UnitController();

        UnitController.activeUnits.Add(new BaseUnit(new Vector2(50, 50), new Vector2(1, 1), 0.7f, .1f, 1f));

        renderer = new ClientRenderer(material);
    }
    private void Update()
    {
        renderer.UpdateBuffer();
        tileController.CheckActiveTiles();
    }
    private void FixedUpdate()
    {
        unitController.HandleUnitMovement();
    }
}
