using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class TrackRandomGenerator : TrackGeneratorStrategy
{
    public int steps;

    public int strategyChangeMinPoints;
    public int strategyChangeMaxPoints;

    private int currentStrategyPoints = 0;
    
    public List<TrackGeneratorStrategy> strategies;

    private TrackGeneratorStrategy currentStrategy;

    public List<Vector3> GeneratePoints(Vector3 from)
    {
        currentStrategyPoints = UnityEngine.Random.Range(strategyChangeMinPoints, strategyChangeMaxPoints);
        currentStrategy = this;

        var currentDescentPerStep = descentPerStep;
        var currentStepDistanceMin = stepDistanceMin;
        var currentStepDistanceMax = stepDistanceMax;
        var currentMinRandomOffset = minRandomOffset;
        var currentMaxRandomOffset = maxRandomOffset;
        
        var points = new List<Vector3>();
        points.Add(from);

        for (var i = 0; i < steps; i++)
        {
            var lastPoint = points[i];
            
            var newPoint = lastPoint + new Vector3(0, currentDescentPerStep, UnityEngine.Random.Range(currentStepDistanceMin, currentStepDistanceMax));

            var offset = new Vector3(UnityEngine.Random.Range(currentMinRandomOffset.x, currentMaxRandomOffset.x), UnityEngine.Random.Range(currentMinRandomOffset.y, currentMaxRandomOffset.y),0);

            var offsetScale = new Vector3(UnityEngine.Random.Range(0, 1f) < 0.5 ? -1 : 1, UnityEngine.Random.Range(0, 1f) < 0.5 ? -1 : 1, 1);

            offset.x *= offsetScale.x;
            offset.y *= offsetScale.y;
            
            newPoint += offset;
            points.Add(newPoint);

            currentStrategyPoints--;

            if (currentStrategyPoints < 0)
            {
                // change strategy...
                currentStrategyPoints = UnityEngine.Random.Range(strategyChangeMinPoints, strategyChangeMaxPoints);

                var currentStrategy = PickRandomStrategy();

                currentDescentPerStep = currentStrategy.descentPerStep;
                currentStepDistanceMin = currentStrategy.stepDistanceMin;
                currentStepDistanceMax = currentStrategy.stepDistanceMax;
                currentMinRandomOffset = currentStrategy.minRandomOffset;
                currentMaxRandomOffset = currentStrategy.maxRandomOffset;
            }
        }
        
        return points;
    }

    private TrackGeneratorStrategy PickRandomStrategy()
    {
        foreach (var strategy in strategies)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < strategy.probabiltiy)
                return strategy;
        }
        return this;
    }
    
}