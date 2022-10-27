using SaberTest;

namespace NUnitTests
{
    public class DeserializeTests
    {
        ListRandom list;
        [SetUp]
        public void Setup()
        {
            list = new();
        }

        [Test]
        public void EmptyStream_ThrowsException()
        {
            var ex = Assert.Throws<Exception>(() => list.Deserialize(Stream.Null));
            Assert.That(ex.Message, Is.EqualTo("Поток не содержит данных"));
        }

        [Test]
        public void Serializing_DeserializedList_ReturnsSameJsonString()
        {
            string inpJsonString = "[\r\n\t{\r\n\t\t\"Previous\": null,\r\n\t\t\"Next\": 1,\r\n\t\t\"Random\": 4,\r\n\t\t\"Data\": \"1\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 0,\r\n\t\t\"Next\": 2,\r\n\t\t\"Random\": 1,\r\n\t\t\"Data\": \"2\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 1,\r\n\t\t\"Next\": 3,\r\n\t\t\"Random\": 3,\r\n\t\t\"Data\": \"3\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 2,\r\n\t\t\"Next\": 4,\r\n\t\t\"Random\": 2,\r\n\t\t\"Data\": \"4\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 3,\r\n\t\t\"Next\": null,\r\n\t\t\"Random\": 1,\r\n\t\t\"Data\": \"5\"\r\n\t}\r\n]"; ;
            Serializer.JsonToList(inpJsonString, list);
            string outJsonString = Serializer.ListToJson(list);
            Assert.That(outJsonString, Is.EqualTo(inpJsonString));
        }
    }
}