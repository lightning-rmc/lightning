import { Injectable } from '@angular/core';
import { Node } from './models/Node';

@Injectable({
	providedIn: 'root',
})
export class NodesService {

	nodes: Node[] = [
		{
			id: 'test1',
			name: 'Beamer links',
			state: 'online',
		},
		{
			id: 'test2',
			name: 'Beamer mitte',
			state: 'offline',
		},
		{
			id: 'test3',
			name: 'Beamer rechts',
			state: 'error',
		},
		{
			id: 'test4',
			name: 'Beamer hinten',
			state: 'syncing',
		},
		{
			id: 'test5',
			name: 'Node mit sehr langem Namen der wahrscheinlich nicht auf die UI passt',
			state: 'syncing',
		},
	];


	async getNodes(): Promise<Node[]> {
		return this.nodes;
	}
}
