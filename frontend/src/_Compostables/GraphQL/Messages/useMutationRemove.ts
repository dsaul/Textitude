import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import isEmpty from '@/Utility/isEmpty';
import type { ISMSMessage } from './ISMSMessage';

const useMutationRemove = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation e164Post($payload: String!) {
			settings {
				e164 {
					remove(e164: $payload) {
						status
					}
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationRemove error', error);
	});
	
	const fn = (payload: ISMSMessage | null) => {
		
		if (payload === null) {
			console.warn('payload === null');
			return;
		}
		if (isEmpty(payload.id)) {
			console.warn('isEmpty(payload.e164)');
			return;
		}
		
		mutate({
			payload: payload.id,
		});
	};
	
	return {
		remove: fn,
		removeLoading: loading,
		removeError: onError,
		removeDone: onDone,
	}
}

export { useMutationRemove }