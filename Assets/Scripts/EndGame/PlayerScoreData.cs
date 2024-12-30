using Unity.Netcode;
using Unity.Collections;

public struct PlayerScoreData : INetworkSerializable
{
    public FixedString128Bytes playerName;
    public float score;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref score);
    }
}
