using System;
using System.Collections.Generic;
using System.Linq;
using Cloth.Mesh;
using Cloth.Provider;
using Cloth.Springs;
using Cloth.Technique;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Cloth.Behaviour
{
    [AddComponentMenu("Matt/Cloth")]
    [RequireComponent(typeof(MeshFilter))]
    public class Cloth : MonoBehaviour
    {
        [Header("Cloth Parameters")] [SerializeField]
        private float stretchK;

        [SerializeField] private float stretchKd;
        [SerializeField] private float shearK;
        [SerializeField] private float shearKd;
        [SerializeField] private float bendK;
        [SerializeField] private float bendKd;
        [SerializeField] private float surfaceDensity;
        [SerializeField] private Vector3 gravity;
        
        [Header("Simulation Settings")]
        [SerializeField] private bool doSimulation;
        [SerializeField] private int numberOfSubsteps;
        [SerializeField] private float cutoffAverage;
        [SerializeField] private bool relaxationMode;
        [SerializeField] private int[] constrainedIndices;
        public int[] ConstrainedIndices
        {
            get => constrainedIndices;
            set => constrainedIndices = value;
        }

        [Header("Debug")] [SerializeField] private bool showDebugOutput;

        [Header("View")]
        [SerializeField] private TMP_Text statusLabel;

        [SerializeField] private UnityEvent onDone;

        private MassSpring _massSpring;
        private MeshFilter _meshFilter;
        private readonly SimulationState _simulationState = new();

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            var mesh = _meshFilter.sharedMesh;
            var vertices = mesh.vertices.Select(_meshFilter.gameObject.transform.TransformPoint).ToArray();

            var massProvider = new MassProvider(surfaceDensity);
            var springProvider = new SpringProvider();
            _massSpring = new MassSpring(massProvider, springProvider, mesh.triangles, vertices);
            _massSpring.ConstrainedIndices.AddRange(constrainedIndices);
        }

        private void FixedUpdate()
        {
            if (!doSimulation) return;

            _simulationState.ShowDebugOutput = showDebugOutput;
            
            var externalForces = relaxationMode ? 
                Array.Empty<Vector3>() : 
                _massSpring.Masses.Select(m => gravity * m).ToArray();

            var subStepTime = Time.fixedDeltaTime / numberOfSubsteps;

            for (var i = 0; i < numberOfSubsteps; i++)
            {
                _massSpring.Step(subStepTime, externalForces);

                if (relaxationMode)
                {
                    _massSpring.ResetVelocities();
                }
            }

            if (_simulationState.IsDone(_massSpring.Positions, cutoffAverage, Time.fixedDeltaTime))
            {
                statusLabel.text = "Done!";
                statusLabel.color = Color.green;
                Debug.Log("Done!");
                doSimulation = false;
                
                onDone.Invoke();
            }

            MeshUpdater.UpdateMeshes(_meshFilter, null, _massSpring.Positions);
        }

        private void OnDrawGizmos()
        {
            _massSpring?.OnDrawGizmos();

            if (_massSpring == null)
            {
                return;
            }

            foreach (var index in constrainedIndices)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_massSpring.Positions[index], 0.2f);
            }
        }

        public void ToggleSimulation()
        {
            doSimulation = !doSimulation;

            if (statusLabel)
            {
                statusLabel.text = doSimulation ? "Going" : "Stopped";
                statusLabel.color = Color.red;
            }

            if (doSimulation && _massSpring != null)
            {
                _massSpring.StretchK = stretchK;
                _massSpring.StretchKd = stretchKd;
                _massSpring.ShearK = shearK;
                _massSpring.ShearKd = shearKd;
                _massSpring.BendK = bendK;
                _massSpring.BendKd = bendKd;
                
                // Update the underlying indices only when we need to
                _massSpring.ConstrainedIndices = constrainedIndices.ToList();
            }
        }

        public void MatchClothPositionsToMesh()
        {
            var mesh = _meshFilter.sharedMesh;
            var vertices = mesh.vertices.Select(_meshFilter.gameObject.transform.TransformPoint).ToArray();
            
            // Don't need to update springs, because they were created based on default cloth position.
            _massSpring.UpdatePositions(vertices);
        }
        
        private class SimulationState
        {
            public bool ShowDebugOutput { get; set; }
            [CanBeNull] private List<Vector3> _previousPositions;
            private float _elapsed;
            
            public bool IsDone(Vector3[] positions, float cutoff, float dt)
            {
                _elapsed += dt;
                
                if (_previousPositions == null)
                {
                    _previousPositions = positions.Select(s => s).ToList();
                    return false;
                }
                
                var differences = _previousPositions.Zip(positions, Vector3.Distance).ToList();
                _previousPositions = positions.Select(s => s).ToList();

                if (ShowDebugOutput)
                {
                    Debug.Log($"Stats: Avg {differences.Average()}, Min {differences.Min()}, Max {differences.Max()}");    
                }

                // Including time just in case the first few frames have really small movements.
                return differences.Average() < cutoff && _elapsed > 0.5f;
            }
        }
    }
}