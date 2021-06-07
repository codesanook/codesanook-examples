import * as components from './components'

// Export all React components inside a component folder

declare var global: any;
// Export as a module name to not be overridden by other modules


global.components = components;
