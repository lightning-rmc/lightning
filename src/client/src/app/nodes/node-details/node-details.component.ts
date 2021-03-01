import { Component, Input, OnInit } from '@angular/core';
import { Node } from '../models/Node.type';
import { NodeState } from '../models/NodeState.enum';

@Component({
	selector: 'app-node-details',
	templateUrl: './node-details.component.html',
	styleUrls: ['./node-details.component.scss'],
})
export class NodeDetailsComponent implements OnInit {
	@Input() node?: Node;

	constructor() {}

	ngOnInit(): void {}

	getStateName(state?: NodeState): string {
		switch (state) {
			case NodeState.Edit:
				return 'EDIT';
			case NodeState.Live:
				return 'LIVE';
			case NodeState.Info:
				return 'INFO';
			case NodeState.Offline:
				return 'OFFLINE';
			case NodeState.Error:
				return 'ERROR';
			default:
				return 'UNKNOWN';
		}
	}
}
