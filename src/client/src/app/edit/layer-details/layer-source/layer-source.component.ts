import { Component, Input, OnInit } from '@angular/core';
import { Layer } from 'src/app/Project.type';

@Component({
	selector: 'app-layer-source',
	templateUrl: './layer-source.component.html',
	styleUrls: ['./layer-source.component.scss'],
})
export class LayerSourceComponent implements OnInit {
	constructor() {}

	@Input()
	layer!: Layer;

	ngOnInit(): void {}
}
