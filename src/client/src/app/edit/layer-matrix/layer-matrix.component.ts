import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LayerGroup } from 'src/app/Project.type';

@Component({
	selector: 'app-layer-matrix',
	templateUrl: './layer-matrix.component.html',
	styleUrls: ['./layer-matrix.component.scss'],
})
export class LayerMatrixComponent {
	@Input()
	groups?: LayerGroup[];

	@Output()
	addLayer = new EventEmitter<string>();

	@Output()
	addGroup = new EventEmitter<string>();

	@Output()
	deleteLayer = new EventEmitter<string>();

	@Output()
	deleteGroup = new EventEmitter<string>();
}
