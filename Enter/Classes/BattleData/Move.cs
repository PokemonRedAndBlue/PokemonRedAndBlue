using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;

public enum MoveCategory
{
    Physical,
    Special
}

public class Move
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int Power { get; set; }
    public MoveCategory Category { get; set; }
}
