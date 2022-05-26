using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.UI;
public class Cutter : MonoBehaviour
{
    private GameManager gameManager;
    public Text lowerHullWeightText;
    
    Material material;
    GameObject objectToCut;

    private int tolerance;
    private int targetWeight;

    public AudioClip cutSoundEffect;
    private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cuttable"))
        {
            material = other.GetComponent<MeshRenderer>().material;
            objectToCut = other.gameObject;
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        lowerHullWeightText.text = " ";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (objectToCut != null)
            {
                SlicedHull cutObject = Cut(objectToCut, material);

                Mesh mesh = objectToCut.GetComponent<MeshFilter>().mesh;
                float volume = objectToCut.GetComponent<MeshVolumeCalculater>().VolumeOfMesh(mesh);

                GameObject upperHull = cutObject.CreateUpperHull(objectToCut, material);
                upperHull.AddComponent<UpperHullMovement>();

                GameObject lowerHull = cutObject.CreateLowerHull(objectToCut, material) as GameObject;
                lowerHull.AddComponent<MeshCollider>().convex = true;
                lowerHull.AddComponent<Rigidbody>();
                lowerHull.AddComponent<MeshVolumeCalculater>();
                lowerHull.AddComponent<LowerHullMovement>();

                Mesh lowerHullMesh = lowerHull.GetComponent<MeshFilter>().mesh;
                float lowerHullVolume = lowerHull.GetComponent<MeshVolumeCalculater>().VolumeOfMesh(lowerHullMesh);
                volume = Mathf.Round(lowerHullVolume / volume * 100);
                volume = Mathf.Clamp(volume, 1, 99);
  
                lowerHullWeightText.text = volume.ToString();
                StartCoroutine(nameof(RemoveLowerHullWeightText));

                audioSource.PlayOneShot(cutSoundEffect);

                if (gameManager.targetWeight == volume)
                {  
                    gameManager.money += 3 * gameManager.moneyMultiplier;
                    gameManager.perfectSlices++;
                }

                else if (gameManager.targetWeight - gameManager.tolerance <= volume 
                && volume <= gameManager.targetWeight + gameManager.tolerance)
                {
                    gameManager.money += 2 * gameManager.moneyMultiplier;
                    gameManager.goodSlices++;
                }
                else
                {  
                    gameManager.badSlices++;
                }

                Destroy(objectToCut);

                if (gameManager.lastFood == false)
                {
                    gameManager.BringNextFood();
                }
                else if (gameManager.lastFood == true)
                {
                    gameManager.FinishLevel();
                }
            }
        }
    }
	public SlicedHull Cut(GameObject obj, Material crossSectionMaterial = null)
	{
		return obj.Slice(transform.position, transform.up, crossSectionMaterial);
	}

    private IEnumerator RemoveLowerHullWeightText()
    {
        yield return new WaitForSeconds(0.6f);
        lowerHullWeightText.text = " ";
    }
}
