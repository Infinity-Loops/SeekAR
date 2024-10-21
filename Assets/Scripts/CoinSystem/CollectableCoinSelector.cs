using Niantic.Lightship.Maps.Core.Coordinates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableCoinSelector : MonoBehaviour
{
    public CoinDB coinDatabase;
    public Transform meshRoot;
    public Transform mapRoot;
    private GameCoin selectedCoin = new GameCoin();
    public CollectableCoinName coinName;
    private CollectableCoin collectableCoin;
    public bool dontChooseCoin;
    public string coinToChoose;
    private void Start()
    {
        HandleChooseCoin();
    }

    void HandleChooseCoin()
    {
        collectableCoin = GetComponent<CollectableCoin>();

        if (!dontChooseCoin)
        {
            var spawnableCoins = coinDatabase.coins.FindAll(x => x.notSpawn == false);
            var randomCoin = Random.Range(0, spawnableCoins.Count);
            selectedCoin = spawnableCoins[randomCoin];

            if (PowerupData.toTheMoonActive)
            {
                int randomChance = Random.Range(0, 2);

                if(randomChance == 1)
                {
                    selectedCoin = coinDatabase.coins.Find(x => x.name == "Bitcoin");
                }
            }
        }
        else
        {
            selectedCoin = coinDatabase.coins.Find(x=>x.name == coinToChoose);
        }

        collectableCoin.coin = selectedCoin;
        coinName.OnSelectedCoin(selectedCoin);


        var instantiatedCoin = Instantiate(selectedCoin.model, meshRoot);
        instantiatedCoin.transform.localPosition = selectedCoin.offset;

        HandleSpawnMapRepresentation();
    }

    void HandleSpawnMapRepresentation()
    {
        var instantiatedMapCoin = Instantiate(selectedCoin.model, mapRoot);
        instantiatedMapCoin.gameObject.SetLayerRecursively(6); //Map Layer
        Vector3 objectPosition = transform.position;
        LatLng worldPos = MapSystem.instance.map.SceneToLatLng(objectPosition);
        Vector3 targetLocation = MapSystem.instance.map.LatLngToScene(worldPos);

        instantiatedMapCoin.transform.localScale = Vector3.one * 10f;
        instantiatedMapCoin.transform.position = targetLocation + Vector3.up * 10f;
        instantiatedMapCoin.gameObject.AddComponent<RotatorHandler>();
    }
}
