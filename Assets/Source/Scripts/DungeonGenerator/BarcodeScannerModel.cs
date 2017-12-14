using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Input, Unique, CreateAssetMenu(fileName = "BarcodeScannerModel", menuName = "Databases/BarcodeScannerModel", order = 3)]
public class BarcodeScannerModel : ScriptableObject {
  public GameObject prefab;
}