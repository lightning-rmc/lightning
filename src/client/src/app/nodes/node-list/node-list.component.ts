import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Node } from '../models/Node';
import { NodeState } from '../models/NodeState';

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

	@Output() selectNode = new EventEmitter<Node>();
}
