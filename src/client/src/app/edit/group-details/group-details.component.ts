import { Component, OnInit } from '@angular/core';

@Component({
	templateUrl: './group-details.component.html',
	styleUrls: ['./group-details.component.scss'],
})
export class GroupDetailsComponent implements OnInit {
	constructor() {}

	object = {
		test: {
			value: 2
		},
		test2: 'Hello World'
	}

	ngOnInit(): void {}
}
