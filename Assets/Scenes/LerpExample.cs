// This example creates three primitive cubes. Using linear interpolation, one cube moves along the line between the others.
// Because the interpolation is clamped to the start and end points, the moving cube never passes the end cube, and remains at the end position after the interpolation frame limit is reached.    
// Attach this script to any GameObject in your scene. 

using UnityEngine;

public class LerpExample : MonoBehaviour
{
    // Number of frames in which to completely interpolate between the positions
    int interpolationFramesCount = 300; 
    int elapsedFrames = 0;

    // Number of frames to reset the moving cube to the start position
    int maxFrameReset = 900;

    GameObject CubeStart;
    GameObject CubeEnd;
    GameObject CubeMove;


    void Start()
    {
        // Create the cubes
        CubeStart = GameObject.CreatePrimitive(PrimitiveType.Cube);
        CubeStart.transform.position = new Vector3(-5,0,0);

        CubeEnd = GameObject.CreatePrimitive(PrimitiveType.Cube);
        CubeEnd.transform.position = new Vector3(5,0,0);

        CubeMove = GameObject.CreatePrimitive(PrimitiveType.Cube);
        CubeMove.transform.position =  CubeStart.transform.position;
    }

    void Update()
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

        // Interpolate position of the moving cube, based on the ratio of elapsed frames
        CubeMove.transform.position = Vector3.Lerp(CubeStart.transform.position, CubeEnd.transform.position, interpolationRatio);
        
        // Reset elapsedFrames to zero after it reaches maxFrameReset
        elapsedFrames = (elapsedFrames + 1) % (maxFrameReset);  

    }
}