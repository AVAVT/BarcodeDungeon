using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;


public class BarcodeScanSystem : IExecuteSystem, IInitializeSystem
{
  InputContext _context;
  GameContext _gameContext;
  GameObject _scannerGO;

  public BarcodeScanSystem(Contexts contexts)
  {
    _context = contexts.input;
    _gameContext = contexts.game;
  }

  public void Execute()
  {
    if (_scannerGO == null) return;

    if (CameraScanner.scannedValue != 0)
    {
      Debug.Log("Barcode Value:" + CameraScanner.scannedValue);
      _gameContext.SetGenerator(CameraScanner.scannedValue);
      
      GameObject.Destroy(_scannerGO);
      _scannerGO = null;
    }
  }

  public void Initialize()
  {
    _scannerGO = GameObject.Instantiate(_context.barcodeScannerModel.value.prefab);
  }
}