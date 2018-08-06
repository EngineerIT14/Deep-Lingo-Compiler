/*
Deep Lingo compiler - ContainerOfArrays class for the semantic analysis stage.
2018, ITESM CEM.
* IRVIN EMMANUEL TRUJILLO DÍAZ A01370082
* LUIS FERNANDO ESPINOSA ELIZALDE A01375758

  Description: This class is useful to store in  its attribute 
  an array of n size and also it has a custom Equal, this is useful 
  because this class can be used in a set that want to store unique arrays
  depending on the first value of the array.
  
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
  
  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  
  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;

namespace Deeplingo {
    
    public class ContainerOfArrays
    {
        //attributes
        public List<object> CustomArray { get; set; }
    
        //Constructor
        public ContainerOfArrays(List<object> arrayThatIsGoingToBeStored)
        {
            this.CustomArray = arrayThatIsGoingToBeStored;
        }
    
        //Methods
    
        public override bool Equals(Object obj)
        {
            // Check if the object is a ContainerOfArrays.
            // The initial null check is unnecessary as the cast will result in null
            // if obj is null to start with.
            var containerEvaluated = obj as ContainerOfArrays;
    
            if (containerEvaluated == null)
                // If it is null then it is not equal to this instance.
                return false;
            // Instances are considered equal if the first value of their Custom Array matches, note THAT first value of the array is a string.
            return ((string)this.CustomArray[0]).Equals((string)containerEvaluated.CustomArray[0]);
        }
    
        public override int GetHashCode()
        {
            return 0;
        }
        
        //Class method...
        public static ContainerOfArrays GetAndDeleteElementFromSet(HashSet<ContainerOfArrays> table, string nameElement)
        {
            foreach (ContainerOfArrays element in table)
            {
                if (((string)element.CustomArray[0]).Equals(nameElement))
                {
                    table.Remove(element);
                    return element;
                }
            }
            return null;
        }
    }
}




/*

Example to use this class :v  by IE - Irvin :D lel


class Solution {
   

    public static void Main()
    {                                                    //lo que nos importa es el primer valor de la lista
        var man = new ContainerOfArrays(new List<object>() { "variableX", "lol",1 });
        Console.WriteLine("[" + String.Join(",", man.CustomArray) + "]");
        var man2 = new ContainerOfArrays(new List<object>() { "funcionX", ":v" });
        var man3 = new ContainerOfArrays(new List<object>(){"funcionX", ":ofioef"});  //este es una copia de man y aunque sea diferente de la segunda variable la primera variable es igual y es el que nos importa
        var man4 = new ContainerOfArrays(new List<object>() { "variableY", ":prororo" });
        HashSet<ContainerOfArrays> LaTabla = new HashSet<ContainerOfArrays>();

        Console.WriteLine(LaTabla.Contains(new ContainerOfArrays(new List<object>() { "variableY" })));
        man.CustomArray.Add("tavris rifa");
        Console.WriteLine(LaTabla.Add(man));
        Console.WriteLine(LaTabla.Add(man2));
        Console.WriteLine(LaTabla.Add(man3));
        Console.WriteLine(LaTabla.Add(man4));
       
        Console.WriteLine("\n========INFORMACION DE LA TABLA/SET=========");
        foreach (ContainerOfArrays objeto in LaTabla)
        {
            Console.WriteLine("["+String.Join(",",objeto.CustomArray)+"]:v");
        }
        Console.WriteLine("\n==============================");


        ContainerOfArrays valorObtenido = ContainerOfArrays.GetAndDeleteElementFromSet(LaTabla, "funcionX");

        Console.WriteLine("\n\n========INFORMACION DE LA TABLA/SET=========");
        foreach (ContainerOfArrays objeto in LaTabla)
        {
            Console.WriteLine("[" + String.Join(",", objeto.CustomArray) + "]:v");
        }
        Console.WriteLine("\n==============================");


        Console.WriteLine("Este es el elemento que fue recuperado y borrado del set: [" + String.Join(",", valorObtenido.CustomArray) + "]:v");

    }

}


//output:
[variableX,lol,1]
[variableX,lol,1]
False
True
True
False
True

========INFORMACION DE LA TABLA/SET=========
[variableX,lol,1,tavris rifa]:v
[funcionX,:v]:v
[variableY,:prororo]:v

==============================


========INFORMACION DE LA TABLA/SET=========
[variableX,lol,1,tavris rifa]:v
[variableY,:prororo]:v

==============================
Este es el elemento que fue recuperado y borrado del set: [funcionX,:v]:v

*/