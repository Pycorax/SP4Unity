using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    // Arrow
    [Tooltip("Arrow projectile blueprint")]
    public GameObject ArrowBlueprint;
    [Tooltip("Arrow default pool size")]
    public int ArrowPoolSize = 20;
    private List<GameObject> ArrowPool = new List<GameObject>();

    // Empowered Arrow
    [Tooltip("Empowered Arrow projectile blueprint")]
    public GameObject EmpoweredArrowBlueprint;
    [Tooltip("Empowered Arrow default pool size")]
    public int EmpoweredArrowPoolSize = 2;
    private List<GameObject> EmpoweredArrowPool = new List<GameObject>();

    // Flying Sword
    [Tooltip("Flying Sword projectile blueprint")]
    public GameObject FlyingSwordBlueprint;
    [Tooltip("Flying Sword default pool size")]
    public int FlyingSwordPoolSize = 2;
    private List<GameObject> FlyingSwordPool = new List<GameObject>();

    // Lightning
    [Tooltip("Lightning projectile blueprint")]
    public GameObject LightningBlueprint;
    [Tooltip("Lightning default pool size")]
    public int LightningPoolSize = 20;
    private List<GameObject> LightningPool = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        createArrow(ArrowPoolSize);
        createEmpoweredArrow(EmpoweredArrowPoolSize);
        createFlyingSword(FlyingSwordPoolSize);
        createLightning(LightningPoolSize);
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    private void createArrow(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject arrow = Instantiate(ArrowBlueprint);
            setCommonData(ref arrow);
            ArrowPool.Add(arrow);
        }
    }

    private void createEmpoweredArrow(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject ea = Instantiate(EmpoweredArrowBlueprint);
            setCommonData(ref ea);
            EmpoweredArrowPool.Add(ea);
        }
    }

    private void createFlyingSword(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject fs = Instantiate(FlyingSwordBlueprint);
            setCommonData(ref fs);
            FlyingSwordPool.Add(fs);
        }
    }

    private void createLightning(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject lightning = Instantiate(LightningBlueprint);
            setCommonData(ref lightning);
            LightningPool.Add(lightning);
        }
    }

    public GameObject FetchArrow()
    {
        foreach(GameObject arrow in ArrowPool)
        {
            if(!arrow.activeSelf)
            {
                return arrow;
            }
        }
        return null;
    }

    public GameObject FetchEmpoweredArrow()
    {
        foreach(GameObject ea in EmpoweredArrowPool)
        {
            if (!ea.activeSelf)
            {
                return ea;
            }
        }
        return null;
    }

    public GameObject FetchFlyingSword()
    {
        foreach (GameObject fs in FlyingSwordPool)
        {
            if (!fs.activeSelf)
            {
                return fs;
            }
        }
        return null;
    }

    public GameObject FetchLightning()
    {
        foreach (GameObject lightning in LightningPool)
        {
            if (!lightning.activeSelf)
            {
                return lightning;
            }
        }
        return null;
    }

    public void SetState(bool movement)
    {
        // Set movement state for all projectiles
        foreach (GameObject a in ArrowPool)
        {
            a.GetComponent<Rigidbody2D>().isKinematic = movement;
        }
        foreach (GameObject ea in EmpoweredArrowPool)
        {
            ea.GetComponent<Rigidbody2D>().isKinematic = movement;
        }
        foreach (GameObject fs in FlyingSwordPool)
        {
            fs.GetComponent<Rigidbody2D>().isKinematic = movement;
        }
        foreach (GameObject l in LightningPool)
        {
            l.GetComponent<Rigidbody2D>().isKinematic = movement;
        }
    }

    private void setCommonData(ref GameObject go)
    {
        go.SetActive(false);
        float size = GetComponent<TileMap>().TileSize * 2.0f;
        Vector3 scale = new Vector3(size, size);
        go.transform.localScale = scale;
        go.transform.parent = transform;
    }
}
