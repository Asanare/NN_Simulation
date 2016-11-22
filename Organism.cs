using System;
using System.Collections;
using UnityEngine;

public class Organism : MonoBehaviour
{
    public float energy;


    public float[,] m = new float[2, 2];

    public Color colour;

    public int fov = 110;

    private int n_inputs;

    private int n_hidden1;

    private int n_hidden2;

    private int n_outputs;

    private Rigidbody2D rb;

    private float distance;

    private bool initRandom;

    public float age;
    public bool readyToReproduce = false;
    public bool reproducing = false;
    public float amount;
    public float degree;
    public float feeling = 0;
    public float currentAngle;
    public Vector2 pos
    {
        get;
        set;
    }

    public Matrix weights_ih1
    {
        get;
        set;
    }

    public Matrix weights_h1h2
    {
        get;
        set;
    }

    public Matrix weights_h2o
    {
        get;
        set;
    }

    public void Initialize(ArrayList paramaters)
    {
        ArrayList arrayList = (ArrayList)paramaters[0];
        bool flag = (bool)paramaters[1];
        this.n_inputs = (int)arrayList[0];
        this.n_hidden1 = (int)arrayList[1];
        this.n_hidden2 = (int)arrayList[2];
        this.n_outputs = (int)arrayList[3];
        this.initRandom = flag;
        if (!flag)
        {
            this.weights_ih1 = (Matrix)arrayList[4];
            this.weights_h1h2 = (Matrix)arrayList[5];
            this.weights_h2o = (Matrix)arrayList[6];
            this.energy = (float)arrayList[7];
            this.colour = (Color)arrayList[8];
        }
    }

    private void SetColour()
    {
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 1;
        float r = 1/this.distance;
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 3;
        float g = colour.g;
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 5;
        float b = colour.b ;
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 7;
        this.colour = new Color(r, g, b);
        base.GetComponent<SpriteRenderer>().color = this.colour;
    }

    private void Start()
    {
        this.rb = base.GetComponent<Rigidbody2D>();
        this.rb.gravityScale = 0f;
        age = 0;
        if (this.initRandom)
        {
            UnityEngine.Random.seed = DateTime.Now.Millisecond;
            this.weights_ih1 = Matrix.Randomise(this.n_inputs, this.n_hidden1, 0.0f, 1.0f);
            UnityEngine.Random.seed = DateTime.Now.Millisecond;
            this.weights_h1h2 = Matrix.Randomise(this.n_hidden1, this.n_hidden2, 0.0f, 1.0f);
            UnityEngine.Random.seed = DateTime.Now.Millisecond;
            this.weights_h2o = Matrix.Randomise(this.n_hidden2, this.n_outputs, 0.0f, 1.0f);
            UnityEngine.Random.seed = DateTime.Now.Millisecond;
            this.energy = (float)UnityEngine.Random.Range(25, 500);
            UnityEngine.Random.seed = DateTime.Now.Millisecond;
            this.colour = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }
        
        base.GetComponent<SpriteRenderer>().color = this.colour;
        this.RandomMutation();
        //base.InvokeRepeating("RandomMutation", 1f, 0.1f);
    }

    private void Update()
    {
        Matrix matrix = new Matrix(1, n_inputs);
        age += Time.deltaTime;
        this.distance = 1/global::Environment.ComputeDistance(base.transform.position.x, base.transform.position.y, global::Environment.food_pos.x, global::Environment.food_pos.y);
        float angle = global::Environment.ComputeAngle(base.transform.position.x, base.transform.position.y, global::Environment.food_pos.x, Environment.food_pos.y) * (float)(180/Math.PI);
        matrix[0, 0] = (float)this.distance;
        matrix[0, 1] = (float)angle;
        feeling += -1/distance;
        //matrix[0, 2] = (float)1 / energy;
        //matrix[0, 3] = (float)1 / Environment.pop_size;
        //matrix[0, 4] = 1 / (Environment.width * 5);
        //matrix[0, 5] = 1 / (Environment.height * 5);
        //matrix[0, 6] = 1 / this.transform.position.x;
        //matrix[0, 7] = 1 / this.transform.position.y;
        //matrix[0, 8] = 1 / Environment.food_pos.x;
        //matrix[0, 8] = 1 / Environment.food_pos.y;
        //currentAngle = transform.localEulerAngles.z;
        //matrix[0, 9] =  angle / 360f;

        this.SetColour();
        //this.DecreaseWeights();
        //this.RandomMutation();
        base.GetComponent<SpriteRenderer>().color = this.colour;
        this.UpdateSelf(matrix);
        //if (age > 60 * 10 && energy <= 10)
        //{
        //    Die();
        //}
    }
    float sigmoid(float i)
    {
        return (float)(1/ (1 + Math.Pow(Math.E, -i)));
    }
    private void UpdateSelf(Matrix input)
    {
        Matrix mat = Matrix.ApplyFunction(new Func<float, float>(sigmoid), Matrix.Multiply(input, this.weights_ih1));
        Matrix mat2 = Matrix.ApplyFunction(new Func<float, float>(sigmoid), Matrix.Multiply(mat, this.weights_h1h2));
        Matrix output = Matrix.ApplyFunction(new Func<float, float>(sigmoid), Matrix.Multiply(mat2, this.weights_h2o));
        //if ((float)output[0, 2] >= 0.5f && energy >= 200)
        //{
        //    this.ReproduceAsexually();
        //    reproducing = true;
        //}
        //else
        //{
        //    reproducing = false;
        //}
        //if ((float)output[0, 3] >= 0.5f)
        //{
        //    readyToReproduce = true;
        //}
        //else
        //{
        //    readyToReproduce = false;
        //}
        amount = (float)output[0, 0];
        degree = output[0, 1];
        this.Move(amount, degree);
    }
    private void Move(float amount, float angle)
    {
        #region move
        //if (base.transform.position.x >= (Environment.width * 5) - 1)
        //{
        //    Vector3 position = base.transform.position;
        //    position.x = -((Environment.width * 5) - 1);
        //    base.transform.position = position;
        //}
        //else if (base.transform.position.y >= (Environment.height * 5) - 1)
        //{
        //    Vector3 position2 = base.transform.position;
        //    position2.y = -((Environment.height * 5) - 1);
        //    base.transform.position = position2;
        //}
        //else if (base.transform.position.x <= -((Environment.width * 5) - 1))
        //{
        //    Vector3 position3 = base.transform.position;
        //    position3.x = (Environment.width * 5) - 1;
        //    base.transform.position = position3;
        //}
        //else if (base.transform.position.y <= -((Environment.height * 5) - 1))
        //{
        //    Vector3 position4 = base.transform.position;
        //    position4.y = (Environment.height * 5) - 1;
        //    base.transform.position = position4;
        //} 
        #endregion

        //base.transform.localRotation = Quaternion.Euler(0f, 0f, (float)angle);
        transform.Rotate(new Vector3(0, 0, 1), angle );
        rb.AddForce(transform.up * 500f * Time.deltaTime * amount);
        //base.transform.Translate(Vector3.up * Mathf.Abs(amount) * 100f * Time.deltaTime);
        //this.energy -= 0.01f;
        
        if (this.energy < 0.01f)
        {
            this.Die();
        }
    }

    public void Die()
    {
        Environment.population.Remove(transform.gameObject);
        UnityEngine.Object.Destroy(base.gameObject);
    }
    void OnDestroy()
    {
        Die();
    }

    private void DecreaseWeights()
    {
        this.weights_ih1 = 0.999f * this.weights_ih1;
        this.weights_h1h2 = 0.999f * this.weights_h1h2;
        this.weights_h2o = 0.999f * this.weights_h2o;
    }

    private void IncreaseWeights()
    {
        this.weights_ih1 = 1.1f * this.weights_ih1;
        this.weights_h1h2 = 1.1f * this.weights_h1h2;
        this.weights_h2o = 1.1f * this.weights_h2o;
    }

    private void RandomMutation()
    {
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 1;
        this.weights_ih1 = Matrix.ModifyElementWise(this.weights_ih1, (float)UnityEngine.Random.Range(-0.1f, 0.1f), 0.05f);
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 1000;
        this.weights_h1h2 = Matrix.ModifyElementWise(this.weights_h1h2, (float)UnityEngine.Random.Range(-0.1f, 0.1f), 0.05f);
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 133333;
        this.weights_h2o = Matrix.ModifyElementWise(this.weights_h2o, (float)UnityEngine.Random.Range(-0.1f, 0.1f), 0.05f);
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 1332;
        float r = this.colour.r + UnityEngine.Random.Range(-0.1f, 0.1f);
        UnityEngine.Random.seed = DateTime.Now.Millisecond + -1111;
        float g = this.colour.g + UnityEngine.Random.Range(-0.1f, 0.1f);
        UnityEngine.Random.seed = DateTime.Now.Millisecond + 12;
        float b = this.colour.b + UnityEngine.Random.Range(-0.1f, 0.1f);
        this.colour = new Color(r, g, b);
    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<Food>() != null)
        {
            this.energy += 15f;
            //this.IncreaseWeights();
            feeling+=25;
            this.RandomMutation();
            Environment.SpawnOrganisms(2, transform.position, false, weights_ih1, weights_h1h2, weights_h2o, energy * 0.9f, colour);
            UnityEngine.Object.Destroy(coll.gameObject);
            global::Environment.SpawnFood(1, Vector2.zero, true);
        }
        if (coll.gameObject.GetComponent<Organism>() != null)
        {
            if (readyToReproduce && coll.gameObject.GetComponent<Organism>().readyToReproduce && energy >= 100  &&  coll.gameObject.GetComponent<Organism>().energy >= 100)
            {
                Reproduce(this, coll.gameObject.GetComponent<Organism>());
                energy *= 0.7f;
                feeling += 5f;
                //DecreaseWeights();
                coll.gameObject.GetComponent<Organism>().DecreaseWeights();
                coll.gameObject.GetComponent<Organism>().energy *= 0.8f;
            }
            //energy -= 0.05f;
            //feeling -= 0.01f;
        }
    }

    public void Reproduce(Organism org1, Organism org2)
    {
        ArrayList DNA = new ArrayList();
        bool random = false;
        DNA.Add(UnityEngine.Random.value < 0.5f ? org1.n_inputs : org2.n_inputs);
        DNA.Add(UnityEngine.Random.value < 0.5f ? org1.n_hidden1 : org2.n_hidden1);
        DNA.Add(UnityEngine.Random.value < 0.5f ? org1.n_hidden2 : org2.n_hidden2);
        DNA.Add(UnityEngine.Random.value < 0.5f ? org1.n_outputs : org2.n_outputs);
        DNA.Add(Matrix.MergeMatrices(org1.weights_ih1, org2.weights_ih1));
        DNA.Add(Matrix.MergeMatrices(org1.weights_h1h2, org2.weights_h1h2));
        DNA.Add(Matrix.MergeMatrices(org1.weights_h2o, org2.weights_h2o));
        DNA.Add(((org1.energy + org2.energy) / 2) * 0.5f);
        //DNA.Add(new Color(UnityEngine.Random.value < 0.5f ? org1.colour.r : org2.colour.r, UnityEngine.Random.value < 0.5f ? org1.colour.g : org2.colour.g, UnityEngine.Random.value < 0.5f ? org1.colour.b : org2.colour.b));
        DNA.Add(new Color(1, 1, 1));
        ArrayList p = new ArrayList(){DNA, random};
        Environment.SpawnOrganisms(1, transform.position, p);
    }

    float heaviside(float i)
    {
        if (i < 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    private void ReproduceAsexually()
    {
        feeling += 0.5f;   
        Environment.SpawnOrganisms(1, base.transform.position, false, this.weights_ih1, this.weights_h1h2, this.weights_h2o, this.energy * 0.5f, this.colour);
        this.energy *= 0.6f;
    }

    public void SaveDNA()
    {

    }

}
