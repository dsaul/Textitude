/* eslint no-prototype-builtins: 0 */
interface IObjectWithLocaleCompareMethod<T> {
	localeCompare: (other: IObjectWithLocaleCompareMethod<T>) => number;
}
interface IObjectWithCompareMethod<T> {
	compare: (other: IObjectWithCompareMethod<T>) => number;
}

export default <T>(left: T, right: T): number => {
    
	if (left === null) {
		return -1;
	}
	if (right === null) {
		return 1;
	}
    
	if (typeof left !== typeof right) {
		throw new Error('typeof left !== typeof right');
	}
    
	if (typeof left === 'string' && typeof right === 'string') {
		return left.localeCompare(right);
	}
    
	if ((typeof left === 'number') && (typeof right === 'number')) {
		return Math.sign(left - right);
	}
	
	if ((typeof left === 'object') || (typeof right === 'object')) {
		
		if (
			(left as unknown as IObjectWithLocaleCompareMethod<T>).hasOwnProperty('localeCompare') && 
			(right as unknown as IObjectWithLocaleCompareMethod<T>).hasOwnProperty('localeCompare')
		) {
			return (left as unknown as IObjectWithLocaleCompareMethod<T>).localeCompare(
                right as unknown as IObjectWithLocaleCompareMethod<T>
            );
		} else if (
			(left as unknown as IObjectWithCompareMethod<T>).hasOwnProperty('compare') && 
			(right as unknown as IObjectWithCompareMethod<T>).hasOwnProperty('compare')
		) {
			return (left as unknown as IObjectWithCompareMethod<T>).compare(
                right as unknown as IObjectWithCompareMethod<T>
            );
		}
		
		return JSON.stringify(left).localeCompare(JSON.stringify(right));
	}
    
	console.error('unsupported spaceship compare left', left, 'right', right);
    
	return 0;
};