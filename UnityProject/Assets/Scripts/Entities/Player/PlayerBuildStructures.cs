using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildStructures : MonoBehaviour
{
    [SerializeField] private SO_Structures structuresData;

    private PlayerController _playerController;
    private GameObject _selectedStructure;
    private int _currentCost;
    private Vector3Int _buildPosition;
    private HashSet<Tuple<int,int>> _alreadyBuildPositions;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _alreadyBuildPositions = new HashSet<Tuple<int, int>>();
        
        // Ignoring Nexo position
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                _alreadyBuildPositions.Add(new Tuple<int, int>(i, j));
            }
        }
    }
    
    private void Update()
    {
        if (GameController.Instance.currentGameMode == GameMode.Gameplay && Input.GetKeyDown(KeyCode.Q))
        {
            SelectStructure(0);
        }
        if (GameController.Instance.currentGameMode == GameMode.Gameplay && Input.GetKeyDown(KeyCode.W))
        {
            SelectStructure(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroySelectedStructure();

            StartCoroutine(SetIsBuildingToFalseEndOFFrame());
        }

        if (_selectedStructure != null)
        {
            _playerController.RaycastMouseToGround(out RaycastHit hit);
            _buildPosition = new Vector3Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));
            _selectedStructure.transform.position = _buildPosition;

            if (PlayerHasEnoughMoney() && IsBuildPositionEmpty() && Input.GetMouseButtonDown(0))
            {
                _playerController.isBuilding = false;
                _playerController.money -= _currentCost;
                _selectedStructure = null;
                _alreadyBuildPositions.Add(new Tuple<int, int>(_buildPosition.x, _buildPosition.z));
            }
        }
    }

    private void SelectStructure(int structureIndex)
    {
        DestroySelectedStructure();

        _playerController.isBuilding = true;
        _selectedStructure = Instantiate(structuresData.structures[structureIndex]);
        _currentCost = structuresData.prices[structureIndex];

        if (_currentCost > _playerController.money)
        {
            MakeMaterialRed(_selectedStructure);
        }
    }

    private bool PlayerHasEnoughMoney()
    {
        return _playerController.money >= _currentCost;
    }

    private bool IsBuildPositionEmpty()
    {
        return !_alreadyBuildPositions.Contains(new Tuple<int, int>(_buildPosition.x, _buildPosition.z));
    }

    private void DestroySelectedStructure()
    {
        if (_selectedStructure != null)
        {
            Destroy(_selectedStructure);
            _selectedStructure = null;
        }
    }

    private void MakeMaterialRed(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            foreach (Material material in meshRenderer.materials)
            {
                material.color = Color.red;
            }
        }
    }

    private IEnumerator SetIsBuildingToFalseEndOFFrame()
    {
        yield return new WaitForEndOfFrame();
        _playerController.isBuilding = false;
    }
}
