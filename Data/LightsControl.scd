(

~lightsControlSend.free;
~lightsControlSend = NetAddr.new("127.0.0.1", 5500);

~lightsControl.free;
~lightsControl = {
	var sig = SoundIn.ar(0);
	var volume = Amplitude.ar(sig);
	SendReply.kr(Impulse.kr(8), '/lightscontrolosc', [volume]);

}.play;

~lightsControlOsc.free;
~lightsControlOsc = OSCFunc({
	arg msg;
	~lightsControlSend.sendMsg("/audio", msg[3]);
}, '/lightscontrolosc', s.addr);

)