import { Component, Input, OnInit } from '@angular/core';
import { Layer } from 'src/app/Project.type';

@Component({
	selector: 'app-layer-transform',
	templateUrl: './layer-transform.component.html',
	styleUrls: ['./layer-transform.component.scss'],
})
export class LayerTransformComponent implements OnInit {
	constructor() {}

	@Input()
	layer!: Layer;

	ngOnInit(): void {}
}
