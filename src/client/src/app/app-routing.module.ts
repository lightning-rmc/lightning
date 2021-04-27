import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LiveGuard } from './live/live.guard';

const routes: Routes = [
	{ path: 'nodes', loadChildren: () => import('./nodes/nodes.module').then((m) => m.NodesModule) },
	{ path: 'media', loadChildren: () => import('./media/media.module').then((m) => m.MediaModule) },
	{ path: 'edit', loadChildren: () => import('./edit/edit.module').then((m) => m.EditModule) },
	{ path: 'live', loadChildren: () => import('./live/live.module').then((m) => m.LiveModule), canDeactivate: [LiveGuard] },
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
