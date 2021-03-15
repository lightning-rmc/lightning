import { Component, Input, OnInit } from '@angular/core';
import { Layer } from 'src/app/Project.type';

@Component({
	selector: 'app-layer-blend',
	templateUrl: './layer-blend.component.html',
	styleUrls: ['./layer-blend.component.scss'],
})
export class LayerBlendComponent implements OnInit {
	constructor() {}

	@Input()
	layer!: Layer;

	ngOnInit(): void {}
}
