import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { ControllerService, ControllerState } from '../controller.service';
import { LiveComponent } from './live.component';

@Injectable({ providedIn: 'root' })
export class LiveGuard implements CanDeactivate<LiveComponent> {
	constructor(private controllerService: ControllerService) {}

	canDeactivate(
		component: LiveComponent,
		currentRoute: ActivatedRouteSnapshot,
		currentState: RouterStateSnapshot,
		nextState: RouterStateSnapshot
	) {
		return this.controllerService.state !== ControllerState.Live;
	}
}
