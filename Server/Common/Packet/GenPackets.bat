START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
XCOPY /Y GenPackets.cs "../../DummyClient/Packet"
XCOPY /Y GenPackets.cs "D:\unity_2024\Client\Assets\Scripts\Packet"
XCOPY /Y GenPackets.cs "../../Server/Packet"

XCOPY /Y ClientPacketManager.cs "../../DummyClient/Packet"
XCOPY /Y ClientPacketManager.cs "D:\unity_2024\Client\Assets\Scripts\Packet"
XCOPY /Y ServerPacketManager.cs "../../Server/Packet"