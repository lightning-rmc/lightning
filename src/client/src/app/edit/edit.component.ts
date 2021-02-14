import { Component, OnInit } from '@angular/core';
import { LayerGroup } from '../Project.type';

@Component({
	selector: 'app-edit',
	templateUrl: './edit.component.html',
	styleUrls: ['./edit.component.scss'],
})
export class EditComponent implements OnInit {
	constructor() {}

	groups: LayerGroup[] = [
		{
			id: 'group-1',
			layer: [
				{
					id: 'layer-1',
					source: 'media1.mp4',
				},
				{
					id: 'layer-2',
					source: 'media2.mp4',
				},
				{
					id: 'layer-3',
					source: 'media3.mp4',
				},
				{
					id: 'layer-4',
					source: 'media4.mp4',
				},
			],
		},
		{
			id: 'group-2',
			layer: [
				{
					id: 'layer-5',
					source: 'media1.mp4',
				},
			],
		},
	];

	ngOnInit(): void {}
}
