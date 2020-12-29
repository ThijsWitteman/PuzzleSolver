using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleSolver
{
    public partial class Form1 : Form
    {
        class Field
        {
            public Field(string n)
            {
                name = n;
                state = false;
                neighbours = new List<Field>();
            }

            public bool State()
            {
                return state;
            }

            public void State(bool s)
            {
                state = s;
            }

            public string Name()
            {
                return name;
            }

            readonly string name;
            private bool state;
            public List<Field> neighbours;
        }

        //private string[,] FieldMatrix = new string[3, 3];
        //HARDCCODED !!
        private readonly string[,] FieldMatrix = new string[3,3] { { "button1", "button2", "button3" }, { "button4", "button5", "button6" }, { "button7", "button8", "button9" } };
        List<Field> Fields = new List<Field>();

        public Form1()
        {
            InitializeComponent();
            
            // Set a click event handler for the button in the panel
            //find neighbours
            foreach (var button in this.Controls.OfType<Button>())
            {
                Field field = new Field(button.Name);
                Fields.Add(field);
                button.Click += HandleClick;
            }

            InitNeighbours();
            RandomizeFields();
        }

        void InitNeighbours()
        {
            foreach (Field field in Fields)
            {
                int xCoord = 0;
                int yCoord = 0;

                for (int i = 0; i < FieldMatrix.GetLength(0); i++)
                    for (int j = 0; j < FieldMatrix.GetLength(1); j++)
                        if (FieldMatrix[i, j] == field.Name())
                        {
                            xCoord = i;
                            yCoord = j;
                        }

                for (int i = 0; i < FieldMatrix.GetLength(0); i++)
                    for (int j = 0; j < FieldMatrix.GetLength(1); j++)
                    {
                        int xDif = Math.Abs(xCoord - i);
                        int yDif = Math.Abs(yCoord - j);
                        if ((xDif == 1 && yCoord == j) || (yDif == 1 && xCoord == i))
                            foreach (Field findField in Fields)
                                if (findField.Name() == FieldMatrix[i, j])
                                    field.neighbours.Add(findField);
                    }

            }
        }

        void RandomizeFields()
        {
            Random rng = new Random();
            foreach (Field field in Fields)
            {
                bool state = rng.Next(100) % 2 == 0;
                field.State(state);
            }
            UpdateFields();
        }

        void UpdateFields()
        {
            foreach (var button in this.Controls.OfType<Button>())
            {
                foreach (Field field in Fields)
                {
                    if (field.Name() == button.Name)
                    {
                        if (field.State())
                        {
                            button.BackColor = Color.Blue;
                        }
                        else
                        {
                            button.BackColor = Color.Black;
                        }
                        break;
                    }

                }
                    
            }
            AreAllActive();
        }

        private void HandleClick(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                foreach (Field field in Fields)
                {
                    if (field.Name() == button.Name)
                    {
                        SwitchState(field);
                        //switch the neighbours
                        foreach (Field neighbour in field.neighbours)
                        {
                            SwitchState(neighbour);
                        }
                        break;
                    }

                }
            }
            UpdateFields();
        }

        private void SwitchState(Field field)
        {
            if (field.State())
            {
                field.State(false);
            }
            else
            {
                field.State(true);
            }
        }

        private void AreAllActive()
        {
            bool allTrue = true;
            foreach (Field field in Fields)
            {
                if (!field.State())
                    allTrue = false;
            }

            if (allTrue)
                RandomizeFields();
        }
    }
}
