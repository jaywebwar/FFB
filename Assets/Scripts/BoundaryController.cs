using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    [SerializeField] GameObject [] floorFX;
    [SerializeField] GameObject [] leftWallFX;
    [SerializeField] GameObject [] rightWallFX;
    [SerializeField] GameObject [] backWallFX;
    [SerializeField] GameObject [] frontWallFX;
    [SerializeField] List<GameObject> boundaryInteractablesList;

    [SerializeField] float boundarySensingDistance;

    List<ParticleSystem> _floorParticleSystems = new List<ParticleSystem>();
    List<ParticleSystem> _leftWallParticleSystems = new List<ParticleSystem>();
    List<ParticleSystem> _rightWallParticleSystems = new List<ParticleSystem>();
    List<ParticleSystem> _backWallParticleSystems = new List<ParticleSystem>();
    List<ParticleSystem> _frontWallParticleSystems = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < floorFX.Length; i++)
        {
            Debug.Log(i);
            _floorParticleSystems.Insert(i, floorFX[i].GetComponent<ParticleSystem>());
            _leftWallParticleSystems.Insert(i, leftWallFX[i].GetComponent<ParticleSystem>()); 
            _rightWallParticleSystems.Insert(i, rightWallFX[i].GetComponent<ParticleSystem>());
            _backWallParticleSystems.Insert(i, backWallFX[i].GetComponent<ParticleSystem>());
            _frontWallParticleSystems.Insert(i, frontWallFX[i].GetComponent<ParticleSystem>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBoundaryParticleSystems();
    }

    void UpdateBoundaryParticleSystems()
    {
        for (int i = 0; i < boundaryInteractablesList.Count; i++)
        {
            UpdateBoundaryParticleSystem(boundaryInteractablesList[i], i);
        }
    }

    void UpdateBoundaryParticleSystem(GameObject player, int index)
    {
        //floor particle effects
        if (player.transform.position.y < floorFX[index].transform.position.y + boundarySensingDistance)
        {
            Debug.Log("Near floor");
            float translateZ = player.transform.position.z - floorFX[index].transform.position.z;
            float translateX = player.transform.position.x - floorFX[index].transform.position.x;

            _floorParticleSystems[index].transform.position = new Vector3(floorFX[index].transform.position.x + translateX, -100, floorFX[index].transform.position.z + translateZ);
            var em = _floorParticleSystems[index].emission;
            em.enabled = true;
        }
        else
        {
            var em = _floorParticleSystems[index].emission;
            em.enabled = false;
        }

        //left wall particle effects
        if (player.transform.position.z < leftWallFX[index].transform.position.z + boundarySensingDistance)
        {
            Debug.Log("Near left wall");
            float translateX = player.transform.position.x - leftWallFX[index].transform.position.x;
            float translateY = player.transform.position.y - leftWallFX[index].transform.position.y;

            _leftWallParticleSystems[index].transform.position = new Vector3(leftWallFX[index].transform.position.x + translateX, leftWallFX[index].transform.position.y + translateY, -100);
            var em = _leftWallParticleSystems[index].emission;
            em.enabled = true;
        }
        else
        {
            var em = _leftWallParticleSystems[index].emission;
            em.enabled = false;
        }

        //right wall particle effects
        if (player.transform.position.z > rightWallFX[index].transform.position.z - boundarySensingDistance)
        {
            Debug.Log("Near right wall");
            float translateX = player.transform.position.x - rightWallFX[index].transform.position.x;
            float translateY = player.transform.position.y - rightWallFX[index].transform.position.y;

            _rightWallParticleSystems[index].transform.position = new Vector3(rightWallFX[index].transform.position.x + translateX, rightWallFX[index].transform.position.y + translateY, 100);
            var em = _rightWallParticleSystems[index].emission;
            em.enabled = true;
        }
        else
        {
            var em = _rightWallParticleSystems[index].emission;
            em.enabled = false;
        }

        //back wall particle effects
        if (player.transform.position.x < backWallFX[index].transform.position.x + boundarySensingDistance)
        {
            Debug.Log("Near back wall");
            float translateZ = player.transform.position.z - backWallFX[index].transform.position.z;
            float translateY = player.transform.position.y - backWallFX[index].transform.position.y;

            _backWallParticleSystems[index].transform.position = new Vector3(-100, backWallFX[index].transform.position.y + translateY, backWallFX[index].transform.position.z + translateZ);
            var em = _backWallParticleSystems[index].emission;
            em.enabled = true;
        }
        else
        {
            var em = _backWallParticleSystems[index].emission;
            em.enabled = false;
        }

        //front wall particle effects
        if (player.transform.position.x > frontWallFX[index].transform.position.x - boundarySensingDistance)
        {
            Debug.Log("Near front wall");
            float translateZ = player.transform.position.z - frontWallFX[index].transform.position.z;
            float translateY = player.transform.position.y - frontWallFX[index].transform.position.y;

            _frontWallParticleSystems[index].transform.position = new Vector3(100, frontWallFX[index].transform.position.y + translateY, frontWallFX[index].transform.position.z + translateZ);
            var em = _frontWallParticleSystems[index].emission;
            em.enabled = true;
        }
        else
        {
            var em = _frontWallParticleSystems[index].emission;
            em.enabled = false;
        }
    }
}
