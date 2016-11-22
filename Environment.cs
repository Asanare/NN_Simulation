using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class Environment : MonoBehaviour
{
    public static float height = 10;
    public static float width = 20;
    public static Sprite food_sprite;
    public static Sprite organism_sprite;
    public static Vector2 food_pos { get; set; }
    public int no_organisms;
    public int no_food;
    public static ArrayList population;
    public static int pop_size;
    public Text pop_size_text;
    public int generationTime;
    //[Range(0, 30)]
    //public int timeScale;
    public float timer = 0;
    public int max_pop;
    // Use this for initialization
    void Start()
    {
        population = new ArrayList();
        organism_sprite = Resources.Load<Sprite>("Sprites/organism");
        food_sprite = Resources.Load<Sprite>("Sprites/food");
        //GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //GameObject wall_n = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //GameObject wall_e = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //GameObject wall_s = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //GameObject wall_w = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //floor.transform.localScale = new Vector3(width, 1, height);
        //floor.transform.localRotation = Quaternion.Euler(new Vector3(270, 0, 0));
        SpawnFood(no_food, Vector2.zero, true);
        SpawnOrganisms(no_organisms, Vector2.zero, true, new Matrix(5, 5), new Matrix(5, 5), new Matrix(5, 5));
        //Matrix m = Matrix.Randomise(1, 2, -10, 10);
        //string s = Matrix.Display(m);
        //print(s);
        //print(Matrix.Display(Matrix.ApplyFunction(Math.Tanh, m)));
        ////north wall
        //wall_n.transform.localScale = new Vector3(width, 1, height);
        //wall_n.name = "N";
        //wall_n.transform.localPosition = new Vector3(0, (height*height)/2, 0);
        //wall_n.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        ////east wall
        //wall_e.name = "E";
        //wall_e.transform.localScale = new Vector3(width, 1, height);
        //wall_e.transform.localPosition = new Vector3((width*width)/2, 0, 0);
        //wall_e.transform.localRotation = Quaternion.Euler(new Vector3(0,180, 90));
        ////south wall
        //wall_s.name = "S";
        //wall_s.transform.localScale = new Vector3(width, 1, height);
        //wall_s.transform.localPosition = new Vector3(0, -(height * height) / 2, 0);
        //wall_s.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
        ////west wall
        //wall_w.name = "W";
        //wall_w.transform.localScale = new Vector3(width, 1, height);
        //wall_w.transform.localPosition = new Vector3(-(width*width)/2, 0, 0);
        //wall_n.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
    }


    void Update()
    {
        //Time.timeScale = timeScale;
        timer += Time.deltaTime;
        pop_size = population.Count;
        pop_size_text.text = "Population: " + pop_size.ToString();
        float highestF = 0;
        int index = 0;
        if (timer >= generationTime)
        {
            population.Sort(new OrganismFitnessComparer());
            
            if (population.Count >= max_pop)
            {
                print(population.Count/2);
                for (int j = population.Count/2; j <= population.Count-1; j++)
                {
                    Destroy((population[j] as GameObject).GetComponent<Organism>());
                }
            }
            ArrayList temp = population.Clone() as ArrayList;
            foreach (GameObject go in population)
            {
                Organism organism = go.GetComponent<Organism>();
                Destroy(organism);
            }
            population.Clear();
            timer = 0;
            for (int i = 0; i < temp.Count; i++)
            {
                if (i < (Mathf.Floor(temp.Count / 10)))
                {
                    Organism o = (temp[i] as GameObject).GetComponent<Organism>();
                    Random.seed = (System.DateTime.Now.TimeOfDay.Milliseconds * (i + 3));
                    Vector2 pos = new Vector2(Random.Range((-width * 5) + 1, width * 5), Random.Range((-height * 5) + 1, height * 5));
                    SpawnOrganisms(10, pos, false, o.weights_ih1, o.weights_h1h2, o.weights_h2o, 1000f, o.colour);
                }
            }
            
        }

        //if (pop_size == 0)
        //{
        //    SpawnOrganisms(20, Vector2.zero, true);
        //}
        //if (pop_size < 20)
        //{

        //    foreach (GameObject go in population)
        //    {
        //        Organism organism = go.GetComponent<Organism>();
        //        if (organism.feeling > highestF)
        //        {
        //            highestF = organism.feeling;
        //            index = population.IndexOf(go);
        //        }
        //    }
        //    GameObject gameO = population[index] as GameObject;
        //    Organism org = gameO.GetComponent<Organism>();
        //    SpawnOrganisms(25, org.transform.position, false, org.weights_ih1, org.weights_h1h2, org.weights_h2o, org.energy * 2, org.colour);
        //    //SpawnOrganisms(10, Vector2.zero, true);
        //}
    }
    public static float ComputeDistance(float x1, float y1, float targetX, float targetY)
    {
        return Mathf.Sqrt((Mathf.Pow(targetX - x1, 2) + Mathf.Pow(targetY - y1, 2)));
    }
    public static float ComputeAngle(float x1, float y1, float targetX, float targetY)
    {
        return Mathf.Atan2(targetY - y1, targetX - x1);
    }
    public static void SpawnFood(int amount, Vector2 pos, bool random)
    {
        for (int i = 0; i < amount; i++)
        {
            if (random)
            {
                Random.seed = (System.DateTime.Now.TimeOfDay.Milliseconds * (i + 1));
                pos = new Vector2(Random.Range((-width * 5) + 1, width * 5), Random.Range((-height * 5) + 1, height * 5));
            }
            GameObject f = new GameObject("food");
            f.AddComponent<Food>();
            f.AddComponent<SpriteRenderer>().sprite = food_sprite;
            //f.AddComponent<SpriteRenderer>().sprite = Sprite.Create(food_sprite, new Rect(0,0, food_sprite.width, food_sprite.height), new Vector2(food_sprite.width/2, food_sprite.height/2), 512);
            f.AddComponent<Rigidbody2D>().gravityScale = 0;
            f.AddComponent<BoxCollider2D>();
            f.transform.localScale = new Vector3(2, 2);
            f.transform.position = pos;
            food_pos = pos;
        }
    }

    public static void SpawnOrganisms(int amount, Vector2 pos, bool random, Matrix weight_ih1 = null, Matrix weights_h1h2 = null, Matrix weights_h2o = null, float energy = 0, Color colour = default(Color))
    {
        for (int i = 0; i < amount; i++)
        {
            ArrayList DNA = new ArrayList() { 2, 100, 209, 2 };
            if (random)
            {
                Random.seed = (System.DateTime.Now.TimeOfDay.Milliseconds * (i + 3));
                pos = new Vector2(Random.Range((-width * 5) + 1, width * 5), Random.Range((-height * 5) + 1, height * 5));
            }
            else
            {
                DNA.Add(weight_ih1);
                DNA.Add(weights_h1h2);
                DNA.Add(weights_h2o);
                DNA.Add(energy);
                DNA.Add(colour);
            }
            GameObject o = new GameObject("Org" + (population.Count + 1));
            o.AddComponent<Organism>();
            o.SendMessage("Initialize", new ArrayList() { DNA, random });
            o.AddComponent<SpriteRenderer>().sprite = organism_sprite;
            //f.AddComponent<SpriteRenderer>().sprite = Sprite.Create(food_sprite, new Rect(0,0, food_sprite.width, food_sprite.height), new Vector2(food_sprite.width/2, food_sprite.height/2), 512);
            o.AddComponent<Rigidbody2D>().gravityScale = 0;
            o.AddComponent<BoxCollider2D>();
            o.transform.localScale = new Vector3(2, 2);
            o.transform.position = pos;
            population.Add(o);
        }
    }
    public static void SpawnOrganisms(int amount, Vector2 pos, ArrayList par)
    {

        for (int i = 0; i < amount; i++)
        {
            GameObject o = new GameObject("Org" + (population.Count + 1));
            o.AddComponent<Organism>();
            o.SendMessage("Initialize", par);
            o.AddComponent<SpriteRenderer>().sprite = organism_sprite;
            //f.AddComponent<SpriteRenderer>().sprite = Sprite.Create(food_sprite, new Rect(0,0, food_sprite.width, food_sprite.height), new Vector2(food_sprite.width/2, food_sprite.height/2), 512);
            o.AddComponent<Rigidbody2D>().gravityScale = 0;
            o.AddComponent<BoxCollider2D>();
            o.transform.localScale = new Vector3(2, 2);
            o.transform.position = pos;
            population.Add(o);
        }
    }
}
class OrganismFitnessComparer : IComparer
{
    public int Compare(System.Object a, System.Object b)
    {
        GameObject go1 = a as GameObject;
        GameObject go2 = b as GameObject;
        Organism org1 = go1.GetComponent<Organism>() as Organism;
        Organism org2 = go2.GetComponent<Organism>() as Organism;
        if (org1.feeling > org2.feeling) return -1;
        else if (org1.feeling < org2.feeling) return 1;
        else return 0;
    }

}
