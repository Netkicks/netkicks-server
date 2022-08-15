using LiteNetLib.Utils;

public class Utils
{
    public static NetDataWriter singleWriter;

    public static NetDataWriter GetNetDataWriter(string message)
    {
        if (singleWriter == null)
            singleWriter = new NetDataWriter();
        singleWriter.Reset();
        singleWriter.Put(message);
        return singleWriter;
    }

    public static NetDataWriter GetNetDataWriter(byte type)
    {
        if (singleWriter == null)
            singleWriter = new NetDataWriter();
        singleWriter.Reset();
        singleWriter.Put(type);
        return singleWriter;
    }

    public static int GetNextEmptySlot<T>(T[] array)
    {
        for (int i = 0; i <= array.Length - 1; i++)
        {
            if (array[i] == null)
                return i;
        }

        return 0;
    }
}