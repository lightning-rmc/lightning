import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Node } from '../models/Node.type';
import { NodeState } from '../models/NodeState.enum';

@Component({
	selector: 'app-node-list',
	templateUrl: './node-list.component.html',
	styleUrls: ['./node-list.component.scss'],
})
export class NodeListComponent implements OnInit {
	NodeState = NodeState;

	@Input() nodes?: Node[];
	@Input() selectedNode?: Node;
	@Input() isLoading?: boolean = false;
	@Output() selectNode = new EventEmitter<Node>();

	ngOnInit(): void {}
}
