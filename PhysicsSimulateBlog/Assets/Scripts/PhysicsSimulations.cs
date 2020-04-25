using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhysicsSimulations : MonoBehaviour
{
    [SerializeField] int maxIterations; //max iterations we should run the sim for
    [SerializeField] GameObject striker; 
    [SerializeField] List<GameObject> pieces = new List<GameObject>();
    [SerializeField] Vector2 forceToStriker; // To change direction of force applied on striker
    // Start is called before the first frame update
    void Start()
    {
        //To see how striker will move normally with Unity AutoSimulation Physics just play the scene
        striker.GetComponent<Rigidbody>().AddForce(new Vector3(forceToStriker.x, 0, forceToStriker.y)); 
    }

    [ContextMenu("Run Simulation")]
    void RunSimulation()
    {
        //The first thing to do is disable auto physics simulation
        Physics.autoSimulation = false; 
        if (pieces != null)
        {
            //Cache pieces positions now so we can reset their position once simulation is over
            pieces.ForEach(piece => piece.GetComponent<PiecesController>().GetInitialPositon()); 
        }

        //Add force to striker
        striker.GetComponent<Rigidbody>().AddForce(new Vector3(forceToStriker.x, 0, forceToStriker.y));
        //Run simulation until all pieces and striker have stopped Moving
        for (int i = 0; i < maxIterations; i++)
        {
            //To achieve deterministic physics results, you should pass a fixed step value to Physics.Simulate.
            //Usually, step should be a small positive number.
            //Using step values greater than 0.03 is likely to produce inaccurate results.
            Physics.Simulate(Time.fixedDeltaTime);
            //Checking when Pieces and Striker have stopped moving
            if (pieces.All(piece => piece.GetComponent<PiecesController>().CheckIfSleeping()))
            {
                Debug.Log(i);
            }
        }
        //Enabling auto physics simulation
        Physics.autoSimulation = true;
    }

    //To Reset Postions to InitialPositons
    [ContextMenu("Reset Positions")]
    void ResetPositions()
    {
        if (pieces != null)
        {
            pieces.ForEach(piece => piece.GetComponent<PiecesController>().ResetPositon());
        }
    }
}
