import { NodeState } from './NodeState.type';

export type Node = {
	id: string;
	name: string;
	state: NodeState;
};
