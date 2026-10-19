using UnityEngine;
using TMPro;

public class BoltUI : MonoBehaviour
{
    public TMP_Text loaded;
    public TMP_Text stock;
    public crossbow_bolts bolts;
    public BoltLauncher boltLauncher;

    int loaded_ammo = 0;

    void Update()
    {
        stock.text = bolts.value.ToString();
        loaded_ammo = boltLauncher.isLoaded ? 1 : 0;
        loaded.text = loaded_ammo.ToString();
    }
}
