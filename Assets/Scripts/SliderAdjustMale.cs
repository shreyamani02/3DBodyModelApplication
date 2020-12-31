using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;

public class SliderAdjustMale : MonoBehaviour
{


    public TextAsset pcaDataAsset;
    public TextAsset meanDataAsset;

    public Slider sliderBMI;
    public Slider sliderStature;
    public Slider sliderSittingHeight;
    public Slider sliderAge;

    public Text lbBMI;
    public Text lbStature;
    public Text lbSittingHeight;
    public Text lbAge;


    List<double[]> pcaData = new List<double[]>();


    int predAnthNum = 49;
    int predLandmarkNum = 93;


    double[][] meanVertices;
    Mesh mesh;


    //this for initialization
    void Start()
    {

        var pcaDataStr = pcaDataAsset.text.Split(new char[] { '\n' });

        for (int ncnt = 0; ncnt < pcaDataStr.Length; ncnt++)
        {
            var aline = pcaDataStr[ncnt];
            string[] linedata = aline.Split(new char[] { ',' });

            if (aline == "") continue;
            List<double> adata = new List<double>();
            for (int i = 0; i < linedata.Length; i++)
            {
                if (linedata[i].Contains("\r"))
                    linedata[i] = linedata[i].Replace("\r", string.Empty);

                adata.Add(Convert.ToDouble(linedata[i]));
                //Debug.Log(adata[i]);
            }

            pcaData.Add(adata.ToArray());
            Debug.Log(ncnt);

            if (ncnt == 43610)
                Debug.Log(ncnt);
        }

        var filter = GetComponent<MeshFilter>();
        if (!filter)
            filter = gameObject.AddComponent<MeshFilter>();

        // get mean vertices
        mesh = filter.mesh;

        // make the mesh capable for dynamic change
        mesh.MarkDynamic();

        List<double[]> verts = new List<double[]>();
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            verts.Add(new double[] { mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z });
        }

        meanVertices = verts.ToArray();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ModelAnthroUpdate()
    {
        var newverts = mesh.vertices;
        var newtri = mesh.triangles;
        var newnorm = mesh.normals;

        ////lbStatus.text = pcaData[(int)slider.value];

        // input : {stature (mm), BMI (kg/m^2), Sitting Height/Stature, Age (YO), Age*BMI, 1.0}.
        var Anths = new double[] {
            sliderStature.value,
            sliderBMI.value,
            sliderSittingHeight.value,
            sliderAge.value,
            sliderBMI.value*sliderAge.value,
            1.0
        };

        var skipNum = predAnthNum + predLandmarkNum * 3;
        for (int i = 0; i < meanVertices.Length; i++)
        {
            var diffx = calcCoords(Anths, pcaData[skipNum + i * 3 + 0]);
            var diffy = calcCoords(Anths, pcaData[skipNum + i * 3 + 1]);
            var diffz = calcCoords(Anths, pcaData[skipNum + i * 3 + 2]);

            newverts[i].x = (float)meanVertices[i][0] + diffx;
            newverts[i].y = (float)meanVertices[i][1] + diffy;
            newverts[i].z = (float)meanVertices[i][2] + diffz;
        }

        mesh.Clear();
        mesh.vertices = newverts;
        mesh.triangles = newtri;
        mesh.normals = newnorm;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        lbStature.text = "Stature: " + sliderStature.value.ToString();
        lbBMI.text = "BMI: " + sliderBMI.value.ToString();
        lbSittingHeight.text = "Sitting Height / Stature: " + sliderSittingHeight.value.ToString();
        lbAge.text = "Age: " + sliderAge.value.ToString();

    }

    private float calcCoords(double[] diffAnths, double[] onePCAdata)
    {
        var diffCoords = 0.0;

        for (int k = 0; k < diffAnths.Length; k++)
        {
            diffCoords += onePCAdata[k] * diffAnths[k];
        }

        return ((float)diffCoords);
    }
}
