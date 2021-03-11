import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { Layer } from 'src/app/Project.type';
import { LayerSourceComponent } from './layer-source/layer-source.component';
import { LayerTransformComponent } from './layer-transform/layer-transform.component';

@Component({
	templateUrl: './layer-details.component.html',
	styleUrls: ['./layer-details.component.scss'],
})
export class LayerDetailsComponent implements OnInit {
	constructor() {}

	activeTab?: string;
	layer?: Layer;

	ngOnInit(): void {
	}
}
