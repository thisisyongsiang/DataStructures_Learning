#version 330 core

in vec3 vertexColor;

out vec4 fragColor;

void main(){
	float dist = length(gl_PointCoord-0.5);
	float chk= step(dist,0.5);
	if(chk==1.0){
		fragColor=vec4(vertexColor,0);
	}
	else{
	discard;
	}
}