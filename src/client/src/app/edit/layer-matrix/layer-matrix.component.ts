import { Component, Input, OnInit } from '@angular/core';
import { LayerGroup } from 'src/app/Project.type';

@Component({
	selector: 'app-layer-matrix',
	templateUrl: './layer-matrix.component.html',
	styleUrls: ['./layer-matrix.component.scss'],
})
export class LayerMatrixComponent implements OnInit {
	constructor() {}

	@Input()
	groups?: LayerGroup[];

	ngOnInit(): void {}
}
