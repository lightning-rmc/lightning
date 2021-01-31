import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Node } from '../models/Node.type';
import { NodeState } from '../models/NodeState.enum';

@Component({
	selector: 'app-node-list',
	templateUrl: './node-list.component.html',
	styleUrls: ['./node-list.component.scss'],
})
export class NodeListComponent implements OnInit {
	constructor() {}

	@Input() nodes?: Node[];

	ngOnInit(): void {}
	NodeState = NodeState;
}
