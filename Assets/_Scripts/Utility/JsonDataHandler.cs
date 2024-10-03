using System.IO;
using Sirenix.Serialization;

public class JsonDataHandler<T>
{
	private const string extension = ".json";

	public void Save(T data, string path)
	{
		byte[] serializedData = SerializationUtility.SerializeValue(data, DataFormat.JSON);
		File.WriteAllBytes(path + extension, serializedData);
	}

	public T Load(string path)
	{
		if (!File.Exists(path + extension))
			return default;

		byte[] bytes = File.ReadAllBytes(path + extension);
		return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
	}
}
