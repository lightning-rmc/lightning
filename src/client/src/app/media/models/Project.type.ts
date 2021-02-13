export interface Project {
	media: Media[];
	nodes: Node[],
	layerGroups: LayerGroup[];
}

export interface Media {
	id: string;
	name: string;
}

export interface Node {
	id: string;
	name: string;
	state: any;
	properties: {
		framesPerSecond: number;
		resulution: {
			width: number;
			height: number;
		}
	}
}

export interface LayerGroup {
	id: string;
	layer: Layer[];
	nodes: Node[];
}

export interface Layer {
	id: string;
	source: string;
	restartAfterStop: boolean;
	transformation: {
		x: number;
		y: number;
		scale: number;
		rotation: number;
	},
	color: any;
	opacity: number;
	blendMode: any;
}
