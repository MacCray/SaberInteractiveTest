using SaberTest;

ListNode node1 = new();
ListNode node2 = new()  { Data = "2", Previous = node1 };
ListNode node3 = new()  { Data = "3", Previous = node2, Random = node1 };
node1.Next = node2;
node1.Random = node3;

node2.Next = node3;
node2.Random = node2;

ListRandom listSer = new() { Head = node1, Tail = node3 };
ListRandom listDeSer = new();

/*using (Stream ms = new MemoryStream())
{
    listSer.Serialize(ms);
    listDeSer.Deserialize(ms);
}*/

using (Stream s = new FileStream("test.json", FileMode.Create, FileAccess.Write))
{
    listSer.Serialize(s);
}

using (Stream s = new FileStream("test.json", FileMode.Open, FileAccess.Read))
{
    listDeSer.Deserialize(s);
}

