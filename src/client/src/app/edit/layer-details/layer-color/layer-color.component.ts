import { Component, Input, OnInit } from '@angular/core';
import { Layer } from 'src/app/Project.type';

@Component({
	selector: 'app-layer-color',
	templateUrl: './layer-color.component.html',
	styleUrls: ['./layer-color.component.scss'],
})
export class LayerColorComponent implements OnInit {
	constructor() {}

	@Input()
	layer!: Layer;

	ngOnInit(): void {}
}
