/** browser dependent definition are aligned to one and the same standard name **/
navigator.getUserMedia = navigator.getUserMedia || navigator.mozGetUserMedia || navigator.webkitGetUserMedia;
window.RTCPeerConnection = window.RTCPeerConnection || window.mozRTCPeerConnection || window.webkitRTCPeerConnection;
window.RTCIceCandidate = window.RTCIceCandidate || window.mozRTCIceCandidate || window.webkitRTCIceCandidate;
window.RTCSessionDescription = window.RTCSessionDescription || window.mozRTCSessionDescription || window.webkitRTCSessionDescription;
window.SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition || window.mozSpeechRecognition
    || window.msSpeechRecognition || window.oSpeechRecognition;

var config = {
    wssHost: getWsUri()
    // wssHost: 'wss://example.com/myWebSocket'
};
var localVideoElem = null,
    remoteVideoElem = null,
    localVideoStream = null,
    videoCallButton = null,
    endCallButton = null;
    process_next = false;
    started = false;
var peerConn = null,
    wsc = new WebSocket(config.wssHost),
    peerConnCfg = {
        'iceServers': [
            { 'urls': 'stun:stun.services.mozilla.com' },
            { 'urls': 'stun:stun.l.google.com:19302' },
        ]
    };

function getWsUri() {

    var path = document.location.pathname.split('/');
    
    return 'wss://' + (document.location.hostname == '' ? 'localhost' : document.location.hostname) + 
            (document.location.port == '' ? '' : ':' + document.location.port) + '/ws';
}

function pageReady() {
    // check browser WebRTC availability 
    if (navigator.getUserMedia) {
        videoCallButton = document.getElementById("videoCallButton");
        endCallButton = document.getElementById("endCallButton");
        localVideo = document.getElementById('localVideo');
        remoteVideo = document.getElementById('remoteVideo');
        videoCallButton.removeAttribute("disabled");
        videoCallButton.addEventListener("click", startChat);
        endCallButton.addEventListener("click", function (evt) {
            wsc.send(JSON.stringify({ "closeConnection": true }));
        });
    } else {
        alert("Sorry, your browser does not support WebRTC!")
    }

    //initiateCall();
    startMedia();
};

function prepareCall() {
    peerConn = new RTCPeerConnection(peerConnCfg);
    // send any ice candidates to the other peer
    peerConn.onicecandidate = onIceCandidateHandler;
    // once remote stream arrives, show it in the remote video element
    peerConn.onaddstream = onAddStreamHandler;
};

// run start(true) to initiate a call
function startMedia() {
    console.log("startMedia");
    prepareCall();
    // get the local stream, show it in the local video element and send it
    navigator.getUserMedia({ "video": true }, function (stream) {
        localVideoStream = stream;
        localVideo.src = URL.createObjectURL(localVideoStream);
        peerConn.addStream(localVideoStream);
    }, function (error) { console.log(error); });
};

function startChat() {
    console.log("startChat");
    if (!started && !process_next) {
        process_next = true;
        buttonState();
        wsc.send(JSON.stringify(MsgFactory.msg_next()));
    }
}

function buttonState() {
    if (started) {
        videoCallButton.setAttribute("disabled", true);
        endCallButton.removeAttribute("disabled");
    } else {
        videoCallButton.removeAttribute("disabled");
        endCallButton.setAttribute("disabled", true);
    }
}

// run start(true) to initiate a call
function initiateCall() {
    //prepareCall();
    // get the local stream, show it in the local video element and send it
    
};

wsc.onmessage = function (evt) {

    //if (!peerConn) answerCall();

    var msg = JSON.parse(evt.data);

    switch (msg.Command) {
        case 2:
            
            break;
        case 3:

            break;
        case 4:

            if (msg.ChatMessage && msg.ChatMessage.Body && msg.ChatMessage.Body.Initiator) {
                createAndSendOffer();
            }

            process_next = false;
            break;
        case 5:
            var body = msg.ChatMessage.Body;
            open_streaming(body);
            break;
    }
};

function open_streaming(signal) {

    if (signal.sdp) {
        console.log("Received SDP from remote peer.");
        console.log(signal.sdp)

        peerConn.setRemoteDescription(new RTCSessionDescription(signal.sdp),
            function () {
                console.log("setRemoteDescription");

                if (signal && signal.sdp && signal.sdp.type == 'offer') {
                    console.log("offer");
                    createAndSendAnswer()
                } else {
                    console.log("Answer");
                }
            },
            function (evt) {
                console.log(evt)
            });
        
    }
    else if (signal.candidate) {
        console.log("Received ICECandidate from remote peer.");
        peerConn.addIceCandidate(new RTCIceCandidate(signal.candidate));
    } else {
        console.log("invalid message");
        console.log(signal);
    }
}

function createAndSendOffer() {
    console.log("createAndSendOffer");
    peerConn.createOffer(
        function (offer) {
            var off = new RTCSessionDescription(offer);
            peerConn.setLocalDescription(new RTCSessionDescription(off),
                function () {
                    wsc.send(JSON.stringify(MsgFactory.sdp(off)));
                },
                function (error) { console.log(error); }
            );
        },
        function (error) {
            console.log("ERROR!!!");
            console.log(error);
        }
    );
};

function createAndSendAnswer() {
    console.log("createAndSendAnswer");
    peerConn.createAnswer(
        function (answer) {
            console.log(answer);
            var ans = new RTCSessionDescription(answer);
            peerConn.setLocalDescription(ans, function () {
                wsc.send(JSON.stringify(MsgFactory.sdp(ans)));
            },
                function (error) { console.log(error); }
            );
        },
        function (error) {
            console.log("ERROR!!!");
            console.log(error);
        }
    );
};

function onIceCandidateHandler(evt) {
    if (!evt || !evt.candidate) return;

    console.log("onIceCandidateHandler " + evt);

    wsc.send(JSON.stringify(MsgFactory.candidate(evt.candidate)));
};

var MsgFactory = {
    candidate: function (candidate_evt) {
        return {
            Command: 5, ChatMessage: {
                Body: { candidate: candidate_evt}
            }
        };
    },

    sdp: function (sdp_evt) {
        return {
            Command: 5, ChatMessage: {
                Body: { sdp: sdp_evt }
            }
        };
    },

    message: function (msg) {
        return {
            Command: 2, ChatMessage: {
                Body: msg
            }
        };
    },

    msg_next: function () {
        return {
            Command: 0
        };
    },
}

function onAddStreamHandler(evt) {
    console.log("onAddStreamHandler");
    console.log(evt);
    videoCallButton.setAttribute("disabled", true);
    endCallButton.removeAttribute("disabled");
    // set remote video stream as source for remote video HTML5 element
    remoteVideo.src = URL.createObjectURL(evt.stream);
};

function endCall() {
    peerConn.close();
    peerConn = null;
    videoCallButton.removeAttribute("disabled");
    endCallButton.setAttribute("disabled", true);
    if (localVideoStream) {
        localVideoStream.getTracks().forEach(function (track) {
            track.stop();
        });
        localVideo.src = "";
    }
    if (remoteVideo) remoteVideo.src = "";
};